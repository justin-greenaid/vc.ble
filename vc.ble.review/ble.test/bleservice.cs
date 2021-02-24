using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using System.Reflection;
using System.Threading;
using System.Text.RegularExpressions;
using Windows.Devices.Bluetooth.Advertisement;

namespace ble.test
{
    public class Bleservice
    {
        // "Magic" string for all BLE devices
        string[] _requestedBLEProperties = { "System.Devices.Aep.DeviceAddress", "System.Devices.Aep.IsConnected", "System.Devices.Aep.Bluetooth.Le.IsConnectable" };

        // BT_Code: Example showing paired and non-paired in a single query.
        //string _aqsAllBLEDevices = "(System.Devices.Aep.ProtocolId:=\"{bb7bb05e-5972-42b5-94fc-76eaa7084d49}\")";

        ObservableCollection<BluetoothLEDeviceDisplay> KnownDevices = new ObservableCollection<BluetoothLEDeviceDisplay>();
        List<DeviceInformation> _deviceList = new List<DeviceInformation>();
        BluetoothLEDevice _selectedDevice = null;

        List<BluetoothLEAttributeDisplay> _services = new List<BluetoothLEAttributeDisplay>();
        BluetoothLEAttributeDisplay _selectedService = null;

        List<BluetoothLEAttributeDisplay> _characteristics = new List<BluetoothLEAttributeDisplay>();
        string _resultCharacteristic = null;

        // Only one registered characteristic at a time.
        List<GattCharacteristic> _subscribers = new List<GattCharacteristic>();

        // Current data format
        DataFormat _dataFormat = DataFormat.Dec;
        TimeSpan _timeout = TimeSpan.FromSeconds(3);
       
        public Bleservice()
        {
            Console.WriteLine("instance create");
        }
        public ERROR_CODE StartScan()
        {
            var watcher = DeviceInformation.CreateWatcher(BluetoothLEDevice.GetDeviceSelectorFromConnectionStatus(BluetoothConnectionStatus.Disconnected), _requestedBLEProperties, DeviceInformationKind.AssociationEndpointContainer);
            watcher.Added += (DeviceWatcher arg1, DeviceInformation devInfo) =>
            {
                if (_deviceList.FirstOrDefault(d => d.Id.Equals(devInfo.Id) || d.Name.Equals(devInfo.Name)) == null) _deviceList.Add(devInfo);
            };
            watcher.Updated += (_, __) => { }; // We need handler for this event, even an empty!
            //Watch for a device being removed by the watcher
            //watcher.Removed += (DeviceWatcher sender, DeviceInformationUpdate devInfo) =>
            //{
            //    _deviceList.Remove(FindKnownDevice(devInfo.Id));
            //};
 
            watcher.EnumerationCompleted += (DeviceWatcher arg1, object arg) => { arg1.Stop(); };
            watcher.Stopped += (DeviceWatcher arg1, object arg) => { _deviceList.Clear(); arg1.Start(); };
            watcher.Start();
            KnownDevices.Clear();
            return ERROR_CODE.BLE_FOUND_DEVICE;
        }
        public ERROR_CODE StartScan(string devName, Action<string> callback)
        {
            AutoResetEvent autoEvent = new AutoResetEvent(false);
            var watcher = DeviceInformation.CreateWatcher(BluetoothLEDevice.GetDeviceSelectorFromConnectionStatus(BluetoothConnectionStatus.Disconnected), _requestedBLEProperties, DeviceInformationKind.AssociationEndpointContainer);
            watcher.Added += (DeviceWatcher arg1, DeviceInformation devInfo) =>
            {
                if (devInfo.Name.Equals(devName))
                {
                    if (_deviceList.FirstOrDefault(d => d.Id.Equals(devInfo.Id) || d.Name.Equals(devInfo.Name)) == null) _deviceList.Add(devInfo);
                    callback($"Found {devName}");
                    watcher.Stop();
                    autoEvent.Set();
                }
            };
            watcher.Updated += (_, __) => { }; // We need handler for this event, even an empty!
            //Watch for a device being removed by the watcher
            //watcher.Removed += (DeviceWatcher sender, DeviceInformationUpdate devInfo) =>
            //{
            //    _deviceList.Remove(FindKnownDevice(devInfo.Id));
            //};
            watcher.EnumerationCompleted += (DeviceWatcher arg1, object arg) => { arg1.Stop(); };
            //watcher.Stopped += (DeviceWatcher arg1, object arg) => { _deviceList.Clear(); arg1.Start(); };
            watcher.Stopped += (DeviceWatcher arg1, object arg) => { callback("Scan Stopped"); };
            watcher.Start();
            KnownDevices.Clear();
            autoEvent.WaitOne(5000);
            if (_deviceList.Count == 0)
                return ERROR_CODE.NO_SELECTED_SERVICE;
            return ERROR_CODE.BLE_FOUND_DEVICE;
        }
        public void CloseDevice()
        {
            // Remove all subscriptions
            // if (_subscribers.Count > 0) Unsubscribe("all");

            if (_selectedDevice != null)
            {
                _services?.ForEach((s) => { s.service?.Dispose(); });
                _services?.Clear();
                _characteristics?.Clear();
                _selectedDevice?.Dispose();
            }
            _deviceList?.Clear();
        }


        public ERROR_CODE ConnnectionStatus(string deviceName)
        {
            ERROR_CODE result = ERROR_CODE.BLE_NO_CONNECTED;
            if (_selectedDevice == null)
            {
                return ERROR_CODE.BLE_NO_CONNECTED;
            }
            if (_selectedDevice.Name.Equals(deviceName) && _selectedDevice.ConnectionStatus == BluetoothConnectionStatus.Connected)
            {
                result = ERROR_CODE.BLE_CONNECTED;
            }
            return result;
        }

        public string getCharacteristic()
        {
            return _resultCharacteristic;
        }

        public string GetErrorString(ERROR_CODE param)
        { 
            return "ERROR_CODE." + param.ToString();
        }
        /// <summary>
        /// This function reads data from the specific BLE characteristic 
        /// </summary>
        /// <param name="param"></param>
        public async Task<string> ReadCharacteristic(string devName, string param)
        {
            string task_result = GetErrorString(ERROR_CODE.UNKNOWN_ERROR);
            try
            {
                if (ConnnectionStatus(devName) != ERROR_CODE.BLE_CONNECTED)
                {
                    task_result = GetErrorString(ERROR_CODE.BLE_NO_CONNECTED);
                    Console.WriteLine("No BLE device connected.");
                    return task_result;
                }
                if (string.IsNullOrEmpty(param))
                {
                    task_result = GetErrorString(ERROR_CODE.CMD_WRONG_PARAMETER);
                    Console.WriteLine("Nothing to read, please specify characteristic name or #.");
                    return task_result;
                }

                List<BluetoothLEAttributeDisplay> chars = new List<BluetoothLEAttributeDisplay>();

                string charName = string.Empty;
                var parts = param.Split('/');
                // Do we have parameter is in "service/characteristic" format?
                if (parts.Length == 2)
                {
                    string serviceName = Utilities.GetIdByNameOrNumber(_services, parts[0]);
                    charName = parts[1];

                    // If device is found, connect to device and enumerate all services
                    if (!string.IsNullOrEmpty(serviceName))
                    {
                        var attr = _services.FirstOrDefault(s => s.Name.Equals(serviceName));
                        IReadOnlyList<GattCharacteristic> characteristics = new List<GattCharacteristic>();

                        try
                        {
                            // Ensure we have access to the device.
                            var accessStatus = await attr.service.RequestAccessAsync();
                            if (accessStatus == DeviceAccessStatus.Allowed)
                            {
                                var result = await attr.service.GetCharacteristicsAsync(BluetoothCacheMode.Uncached);
                                if (result.Status == GattCommunicationStatus.Success)
                                    characteristics = result.Characteristics;
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"READ_EXCEPTION_2. Can't read characteristics: {ex.Message}");
                            task_result = GetErrorString(ERROR_CODE.READ_EXCEPTION_2);
                        }

                        foreach (var c in characteristics)
                            chars.Add(new BluetoothLEAttributeDisplay(c));
                    }
                }
                else if (parts.Length == 1)
                {
                    if (_selectedService == null)
                    {
                        Console.WriteLine("No service is selected.");
                        task_result = GetErrorString(ERROR_CODE.NO_SELECTED_SERVICE);
                    }
                    chars = new List<BluetoothLEAttributeDisplay>(_characteristics);
                    charName = parts[0];
                }

                // Read characteristic
                if (chars.Count == 0)
                {
                    Console.WriteLine("No Characteristics");
                    task_result = GetErrorString(ERROR_CODE.READ_NOTHING_TO_READ);
                    return task_result;
                }
                if (chars.Count > 0 && !string.IsNullOrEmpty(charName))
                {
                    string useName = Utilities.GetIdByNameOrNumber(chars, charName);
                    var attr = chars.FirstOrDefault(c => c.Name.Equals(useName));
                    if (attr != null && attr.characteristic != null)
                    {
                        // Read characteristic value
                        GattReadResult result = await attr.characteristic.ReadValueAsync(BluetoothCacheMode.Uncached);

                        if (result.Status == GattCommunicationStatus.Success)
                        {
                            Console.WriteLine(Utilities.FormatValue(result.Value, _dataFormat));
                            _resultCharacteristic = Utilities.FormatValue(result.Value, _dataFormat);
                            task_result = GetErrorString(ERROR_CODE.NONE) + " " + _resultCharacteristic;
                        }
                        else
                        {
                            Console.WriteLine($"Read failed: {result.Status}");
                            task_result = GetErrorString(ERROR_CODE.READ_FAIL);
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Invalid characteristic {charName}");
                        task_result = GetErrorString(ERROR_CODE.READ_INVALID_CHARACTERISTIC);
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"READ_EXCEPTION_1. Can't read characteristics: {ex.Message}");
                task_result = GetErrorString(ERROR_CODE.READ_EXCEPTION_1);
            }
            return task_result;
        }

        /// <summary>
        /// Set active service for current device
        /// </summary>
        /// <param name="parameters"></param>
        public async Task<ERROR_CODE> SetService(string serviceName)
        {
            ERROR_CODE task_result = ERROR_CODE.NONE;
            if (_selectedDevice != null)
            {
                if (!string.IsNullOrEmpty(serviceName))
                {
                    string foundName = Utilities.GetIdByNameOrNumber(_services, serviceName);

                    // If device is found, connect to device and enumerate all services
                    if (!string.IsNullOrEmpty(foundName))
                    {
                        var attr = _services.FirstOrDefault(s => s.Name.Equals(foundName));
                        IReadOnlyList<GattCharacteristic> characteristics = new List<GattCharacteristic>();

                        try
                        {
                            // Ensure we have access to the device.
                            var accessStatus = await attr.service.RequestAccessAsync();
                            if (accessStatus == DeviceAccessStatus.Allowed)
                            {
                                // BT_Code: Get all the child characteristics of a service. Use the cache mode to specify uncached characterstics only 
                                // and the new Async functions to get the characteristics of unpaired devices as well. 
                                var result = await attr.service.GetCharacteristicsAsync(BluetoothCacheMode.Uncached);
                                if (result.Status == GattCommunicationStatus.Success)
                                {
                                    characteristics = result.Characteristics;
                                    _selectedService = attr;
                                    _characteristics.Clear();
                                    Console.WriteLine($"Selected service {attr.Name}.");

                                    if (characteristics.Count > 0)
                                    {
                                        for (int i = 0; i < characteristics.Count; i++)
                                        {
                                            var charToDisplay = new BluetoothLEAttributeDisplay(characteristics[i]);
                                            _characteristics.Add(charToDisplay);
                                            Console.WriteLine($"#{i:00}: {charToDisplay.Name}\t{charToDisplay.Chars}");
                                        }
                                    }
                                    else
                                    {
                                        task_result = ERROR_CODE.SERVICE_NO_SERVICE;
                                    }
                                }
                                else
                                {
                                    task_result = ERROR_CODE.SERVICE_ACCESS_ERROR;
                                }
                            }
                            // Not granted access
                            else
                            {
                                task_result = ERROR_CODE.SERVICE_NOT_GRANT_ACCEWSS;
                            }
                        }
                        catch (Exception ex)
                        {
                            task_result = ERROR_CODE.SERVICE_CANOT_READ_CHAR;
                        }
                    }
                    else
                    {
                        task_result = ERROR_CODE.SERVICE_INVALID_SERVICE;
                    }
                }
                else
                {
                    task_result = ERROR_CODE.SERVICE_INVALID_SERVICE;
                }
            }
            else
            {
                task_result = ERROR_CODE.BLE_NO_CONNECTED;
            }

            return task_result;
        }

        public async Task<ERROR_CODE> OpenDevice(string deviceName)
        {
            ERROR_CODE task_result = ERROR_CODE.NONE;

            if (string.IsNullOrEmpty(deviceName))
                return task_result;

            var devs = _deviceList.OrderBy(d => d.Name).Where(d => !string.IsNullOrEmpty(d.Name)).ToList();
            string foundId = Utilities.GetIdByNameOrNumber(devs, deviceName);

            _selectedService = null;
            _services.Clear();

            try
            {
                _selectedDevice = await BluetoothLEDevice.FromIdAsync(foundId).AsTask().TimeoutAfter(_timeout);
                //string_result= $"Connecting to {_selectedDevice.Name}.";

                var result = await _selectedDevice.GetGattServicesAsync(BluetoothCacheMode.Uncached);
                if (result.Status == GattCommunicationStatus.Success)
                {
                    //    listStatus.Items.Adde($"Found {result.Services.Count} services:");

                    for (int i = 0; i < result.Services.Count; i++)
                    {
                        var serviceToDisplay = new BluetoothLEAttributeDisplay(result.Services[i]);
                        _services.Add(serviceToDisplay);
                        //        listStatus.Items.Add($"#{i:00}: {_services[i].Name}");
                    }
                }
                else
                {
                    //    listStatus.Items.Add($"Device {deviceName} is unreachable.");
                    task_result = ERROR_CODE.OPENDEVICE_UNREACHABLE;
                }
            }
            catch
            {
                task_result = ERROR_CODE.UNKNOWN_ERROR;
            }
            return task_result;
        }


        public List<string> GetDeviceList()
        {
            List<string> result = null;
            result = _deviceList.OrderBy(d => d.Name).Where(d => !string.IsNullOrEmpty(d.Name)).Select(d => d.Name).ToList();
            return result;
        }

        public List<string> GetServiceList()
        {
            List<string> result = new List<string>();

            if (_selectedDevice == null)
            {
                result.Add("Selected device is null");
                return result;
            }
            if (_selectedDevice.ConnectionStatus == BluetoothConnectionStatus.Disconnected)
            {
                result.Add($"Device {_selectedDevice.Name} is disconnected.");
                return result;
            }
            if (_services.Count() > 0)
            {
                // List all services
                // listStatus.Items.Add("Available services:");
                for (int i = 0; i < _services.Count(); i++)
                {
                    result.Add($"{_services[i].Name}");
                }
            }
            return result;
        }

        public List<string> GetCharacteristicList()
        {
            List<string> result = new List<string>();

            if (_selectedDevice.ConnectionStatus == BluetoothConnectionStatus.Disconnected)
            {
                result.Add($"Device {_selectedDevice.Name} is disconnected.");
                return result;
            }

            if (_services.Count() == 0)
            {
                result.Add($"Device {_selectedDevice.Name} service count is 0 ");
                return result;
            }

            // If service is selected,
            if (_selectedService == null)
            {
                result.Add($"Device {_selectedDevice.Name} service is null");
                return result;
            }

            // List all characteristics
            if (_characteristics.Count == 0)
            {
                result.Add($"Selected {_selectedService.Name}.charateristics.Count is 0");
                return result;
            }

            result.Add("Available characteristics:");
            for (int i = 0; i < _characteristics.Count(); i++)
            {
                //result.Add($"#{i:00}: {_characteristics[i].Name} {_characteristics[i].characteristic.Uuid} \t{_characteristics[i].Chars}");
                result.Add($"{_characteristics[i].Name}");
            }
            return result;
        }

        private ERROR_CODE ConvertErrorCodePairing(DevicePairingResultStatus status)
        {
            var error_status = ERROR_CODE.UNKNOWN_ERROR;
            switch (status)
            {
                case DevicePairingResultStatus.Paired:
                    error_status = ERROR_CODE.PAIRING_SUCCESS;
                    break;

                case DevicePairingResultStatus.NotReadyToPair:
                    error_status = ERROR_CODE.PAIRING_NOT_READY_TO_PAIR;
                    break;

                case DevicePairingResultStatus.NotPaired:
                    error_status = ERROR_CODE.PAIRING_NOT_PAIRED;
                    break;

                case DevicePairingResultStatus.AlreadyPaired:
                    error_status = ERROR_CODE.PAIRING_ALREADY_PAIRED;
                    break;


                case DevicePairingResultStatus.ConnectionRejected:
                    error_status = ERROR_CODE.PAIRING_CONNECTION_REJECTED;
                    break;


                case DevicePairingResultStatus.TooManyConnections:
                    error_status = ERROR_CODE.PAIRING_TOO_MANY_CONNECTIONS;
                    break;


                case DevicePairingResultStatus.HardwareFailure:
                    error_status = ERROR_CODE.PAIRING_HARDWARE_FAILURE;
                    break;


                case DevicePairingResultStatus.AuthenticationTimeout:
                    error_status = ERROR_CODE.PAIRING_AUTHENTICATION_TIMEOUT;
                    break;


                case DevicePairingResultStatus.AuthenticationNotAllowed:
                    error_status = ERROR_CODE.PAIRING_AUTHENTICATIION_NOT_ALLOWED;
                    break;


                case DevicePairingResultStatus.AuthenticationFailure:
                    error_status = ERROR_CODE.PAIRING_AUTHENTICATION_FAILURE;
                    break;


                case DevicePairingResultStatus.NoSupportedProfiles:
                    error_status = ERROR_CODE.PAIRING_NOT_SUPPORTED_PROFILES;
                    break;


                case DevicePairingResultStatus.ProtectionLevelCouldNotBeMet:
                    error_status = ERROR_CODE.PAIRING_PROTEDTION_LEVEL_COULDNOT_BE_MET;
                    break;


                case DevicePairingResultStatus.AccessDenied:
                    error_status = ERROR_CODE.PAIRING_ACCESS_DENIED;
                    break;


                case DevicePairingResultStatus.InvalidCeremonyData:
                    error_status = ERROR_CODE.PAIRING_INVALID_CEREMONYDATA;
                    break;


                case DevicePairingResultStatus.PairingCanceled:
                    error_status = ERROR_CODE.PAIRING_PAIRING_CANCELED;
                    break;


                case DevicePairingResultStatus.OperationAlreadyInProgress:
                    error_status = ERROR_CODE.PAIRING_OPERATION_ALREADY_INPROGRESS;
                    break;


                case DevicePairingResultStatus.RequiredHandlerNotRegistered:
                    error_status = ERROR_CODE.PAIRING_REQUIRED_HANDLER_NOT_REGISTERED;
                    break;


                case DevicePairingResultStatus.RejectedByHandler:
                    error_status = ERROR_CODE.PAIRING_REJECTED_BY_HANDLER;
                    break;


                case DevicePairingResultStatus.RemoteDeviceHasAssociation:
                    error_status = ERROR_CODE.PAIRING_REMOTE_DEVICE_HAS_ASSOCIATION;
                    break;


                case DevicePairingResultStatus.Failed:
                    error_status = ERROR_CODE.PAIRING_FAILED;
                    break;
            }
            return error_status;
        }
        private ERROR_CODE ConvertErrorCodeUnPairing(DeviceUnpairingResultStatus status)
        {
            var error_status = ERROR_CODE.UNKNOWN_ERROR;

            switch (status)
            {
                case DeviceUnpairingResultStatus.AlreadyUnpaired:
                    error_status = ERROR_CODE.ALREADY_UNPAIRED;
                    break;

                case DeviceUnpairingResultStatus.OperationAlreadyInProgress:
                    error_status = ERROR_CODE.UNPAIRE_ALREADY_INPROGRESS;
                    break;
                case DeviceUnpairingResultStatus.Failed:
                    error_status = ERROR_CODE.UNPAIRE_FAILED;
                    break;

                case DeviceUnpairingResultStatus.Unpaired:
                default:
                    error_status = ERROR_CODE.UNPAIRED_SUCCESS;
                    break;
            }
            return error_status;
        }
    private void CustomOnPairingRequested(
                    DeviceInformationCustomPairing sender,
                    DevicePairingRequestedEventArgs args)
        {
            Console.WriteLine("Done Pairing");
            args.Accept("0");
        }

        public async Task<ERROR_CODE> Pairing(string deviceName)
        {
            ERROR_CODE task_result = ERROR_CODE.UNKNOWN_ERROR;

            if (string.IsNullOrEmpty(deviceName))
                return task_result;

            var devs = _deviceList.OrderBy(d => d.Name).Where(d => !string.IsNullOrEmpty(d.Name)).ToList();
            string foundId = Utilities.GetIdByNameOrNumber(devs, deviceName);

            _selectedService = null;
            _services.Clear();

            //try
            {
                _selectedDevice = await BluetoothLEDevice.FromIdAsync(foundId).AsTask().TimeoutAfter(_timeout);
                //string_result= $"Connecting to {_selectedDevice.Name}.";

                
                if (_selectedDevice.DeviceInformation.Pairing.IsPaired) {
                    var result1 = await _selectedDevice.DeviceInformation.Pairing.UnpairAsync();
                    task_result = ConvertErrorCodeUnPairing(result1.Status);
                    if (task_result != ERROR_CODE.UNPAIRED_SUCCESS) {
                        Console.WriteLine($"{result1.Status}");
                        return task_result;
                    }
                }

                if (_selectedDevice.ConnectionStatus == BluetoothConnectionStatus.Disconnected)
                {
                    Console.WriteLine($"{_selectedDevice.Name} Try Pairing");
                    _selectedDevice.DeviceInformation.Pairing.Custom.PairingRequested += CustomOnPairingRequested;

                    //var result1 = await _selectedDevice.DeviceInformation.Pairing.Custom.PairAsync(
                    //      DevicePairingKinds.ConfirmOnly, DevicePairingProtectionLevel.None);
                    var result1 = await _selectedDevice.DeviceInformation.Pairing.Custom.PairAsync(
                          DevicePairingKinds.ConfirmOnly);
                    _selectedDevice.DeviceInformation.Pairing.Custom.PairingRequested -= CustomOnPairingRequested;
                    task_result =  ConvertErrorCodePairing(result1.Status);

                    Console.WriteLine($"{result1.Status}");
                    if (task_result != ERROR_CODE.PAIRING_SUCCESS)
                    {
                        return task_result;
                    }
                }
                else
                {
                    task_result = ERROR_CODE.PAIRING_ALREADY_CONNECTED;
                    return task_result;
                }

                var result = await _selectedDevice.GetGattServicesAsync(BluetoothCacheMode.Uncached);
                if (result.Status == GattCommunicationStatus.Success)
                {
                    //    listStatus.Items.Adde($"Found {result.Services.Count} services:");

                    for (int i = 0; i < result.Services.Count; i++)
                    {
                        var serviceToDisplay = new BluetoothLEAttributeDisplay(result.Services[i]);
                        _services.Add(serviceToDisplay);
                        //        listStatus.Items.Add($"#{i:00}: {_services[i].Name}");
                    }
                }
                else
                {
                    //    listStatus.Items.Add($"Device {deviceName} is unreachable.");
                    task_result = ERROR_CODE.OPENDEVICE_UNREACHABLE;
                }
            }
            //catch
            //{
            //    task_result = ERROR_CODE.UNKNOWN_ERROR;
            //}
            return task_result;
        }

        public async Task<ERROR_CODE> UnPairing(string deviceName)
        {
            ERROR_CODE task_result = ERROR_CODE.UNKNOWN_ERROR;

            if (string.IsNullOrEmpty(deviceName))
                return task_result;

            var devs = _deviceList.OrderBy(d => d.Name).Where(d => !string.IsNullOrEmpty(d.Name)).ToList();
            string foundId = Utilities.GetIdByNameOrNumber(devs, deviceName);



            try
            {
                if (_selectedDevice == null)
                {
                    Console.WriteLine("Selected device is null");
                    task_result = ERROR_CODE.NO_SELECTED_SERVICE;
                    return task_result;
                }
                if (_selectedDevice.ConnectionStatus == BluetoothConnectionStatus.Connected)
                //if (_selectedDevice.DeviceInformation.Pairing.IsPaired)
                {
                    Console.WriteLine($"{_selectedDevice.Name} Try Pairing");
                    var result1 = await _selectedDevice.DeviceInformation.Pairing.UnpairAsync();
                    task_result = ConvertErrorCodeUnPairing(result1.Status);
                    Console.WriteLine($"{result1.Status}");
                    
                }
                else
                {
                    Console.WriteLine($"{_selectedDevice.Name} wasn't paired");
                    task_result = ERROR_CODE.UNPAIR_FAILED_DISCONNECTED;
                }

            }
            catch
            {
                task_result = ERROR_CODE.OPENDEVICE_UNREACHABLE;
            }

            _selectedService = null;
            _services.Clear();

            return task_result;
        }
    }
 }
