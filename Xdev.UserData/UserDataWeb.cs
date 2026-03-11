using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

#if NETFRAMEWORK
using System.Web.Script.Serialization;
#else
using System.Text.Json;
#endif

namespace Xdev.UserData
{
    /// <summary>
    /// Класс для получения информации о пользователе от PHP API
    /// Class for retrieving user information from PHP API
    /// </summary>
    public class UserInfo : IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl;
        private Dictionary<string, object> _cachedData;
        private DateTime _cacheTime;
        private readonly TimeSpan _cacheDuration = TimeSpan.FromSeconds(30); // Кэш на 30 секунд / 30 second cache

#if NETFRAMEWORK
        private readonly JavaScriptSerializer _jsonSerializer;
#endif

        /// <summary>
        /// Конструктор класса UserInfo
        /// Constructor for UserInfo class
        /// </summary>
        /// <param name="apiUrl">
        /// URL вашего PHP скрипта (по умолчанию: http://k90052gj.beget.tech/API/user_data.php)
        /// URL of your PHP script (default: http://k90052gj.beget.tech/API/user_data.php)
        /// </param>
        public UserInfo(string apiUrl = "http://k90052gj.beget.tech/API/user_data.php")
        {
            _apiUrl = apiUrl;
            _httpClient = new HttpClient();
            _httpClient.Timeout = TimeSpan.FromSeconds(10); // Таймаут 10 секунд / 10 second timeout
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "C# UserInfoClient/1.0");

#if NETFRAMEWORK
            _jsonSerializer = new JavaScriptSerializer();
#endif
        }

        #region Приватные методы / Private methods

        /// <summary>
        /// Получить свежие данные (с кэшированием)
        /// Get fresh data (with caching)
        /// </summary>
        private async Task<Dictionary<string, object>> GetDataAsync(bool forceRefresh = false)
        {
            // Если есть кэш и он не устарел, возвращаем его / If cache exists and is not expired, return it
            if (!forceRefresh && _cachedData != null && DateTime.Now - _cacheTime < _cacheDuration)
            {
                return _cachedData;
            }

            try
            {
                var response = await _httpClient.GetAsync(_apiUrl);
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();

#if NETFRAMEWORK
                var fullData = _jsonSerializer.Deserialize<Dictionary<string, object>>(json);
#else
                var fullData = JsonSerializer.Deserialize<Dictionary<string, object>>(json);
#endif

                if (fullData != null && fullData.ContainsKey("data") && fullData["data"] is Dictionary<string, object> data)
                {
                    _cachedData = data;
                    _cacheTime = DateTime.Now;
                    return data;
                }
            }
            catch { }

            return null;
        }

        /// <summary>
        /// Получить данные синхронно
        /// Get data synchronously
        /// </summary>
        private Dictionary<string, object> GetData(bool forceRefresh = false)
        {
            return Task.Run(async () => await GetDataAsync(forceRefresh)).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Безопасное получение значения из словаря
        /// Safely get value from dictionary
        /// </summary>
        private T GetValue<T>(Dictionary<string, object> dict, string key, T defaultValue = default)
        {
            if (dict != null && dict.ContainsKey(key))
            {
                try
                {
                    var value = dict[key];
                    if (value == null) return defaultValue;
                    return (T)Convert.ChangeType(value, typeof(T));
                }
                catch
                {
                    return defaultValue;
                }
            }
            return defaultValue;
        }

        /// <summary>
        /// Получение вложенного словаря
        /// Get nested dictionary
        /// </summary>
        private Dictionary<string, object> GetNestedDict(Dictionary<string, object> dict, string key)
        {
            if (dict != null && dict.ContainsKey(key) && dict[key] is Dictionary<string, object> nested)
            {
                return nested;
            }
            return new Dictionary<string, object>();
        }

        /// <summary>
        /// Получение массива из словаря
        /// Get array from dictionary
        /// </summary>
        private List<object> GetArray(Dictionary<string, object> dict, string key)
        {
            if (dict != null && dict.ContainsKey(key) && dict[key] is List<object> list)
            {
                return list;
            }
            return new List<object>();
        }

        #endregion

        #region Основные методы получения данных / Main data retrieval methods

        /// <summary>
        /// Получить IP-адрес пользователя
        /// Get user's IP address
        /// </summary>
        public string GetIpAddress()
        {
            var data = GetData();
            return GetValue<string>(data, "ip", "Unknown");
        }

        /// <summary>
        /// Получить IP-адрес пользователя асинхронно
        /// Get user's IP address asynchronously
        /// </summary>
        public async Task<string> GetIpAddressAsync()
        {
            var data = await GetDataAsync();
            return GetValue<string>(data, "ip", "Unknown");
        }

        /// <summary>
        /// Получить страну пользователя
        /// Get user's country
        /// </summary>
        public string GetCountry()
        {
            var data = GetData();
            var location = GetNestedDict(data, "location");
            return GetValue<string>(location, "country", "Unknown");
        }

        /// <summary>
        /// Получить страну пользователя асинхронно
        /// Get user's country asynchronously
        /// </summary>
        public async Task<string> GetCountryAsync()
        {
            var data = await GetDataAsync();
            var location = GetNestedDict(data, "location");
            return GetValue<string>(location, "country", "Unknown");
        }

        /// <summary>
        /// Получить город пользователя
        /// Get user's city
        /// </summary>
        public string GetCity()
        {
            var data = GetData();
            var location = GetNestedDict(data, "location");
            return GetValue<string>(location, "city", "Unknown");
        }

        /// <summary>
        /// Получить город пользователя асинхронно
        /// Get user's city asynchronously
        /// </summary>
        public async Task<string> GetCityAsync()
        {
            var data = await GetDataAsync();
            var location = GetNestedDict(data, "location");
            return GetValue<string>(location, "city", "Unknown");
        }

        /// <summary>
        /// Получить регион пользователя
        /// Get user's region
        /// </summary>
        public string GetRegion()
        {
            var data = GetData();
            var location = GetNestedDict(data, "location");
            return GetValue<string>(location, "region", "Unknown");
        }

        /// <summary>
        /// Получить регион пользователя асинхронно
        /// Get user's region asynchronously
        /// </summary>
        public async Task<string> GetRegionAsync()
        {
            var data = await GetDataAsync();
            var location = GetNestedDict(data, "location");
            return GetValue<string>(location, "region", "Unknown");
        }

        /// <summary>
        /// Получить часовой пояс пользователя
        /// Get user's timezone
        /// </summary>
        public string GetTimezone()
        {
            var data = GetData();
            var location = GetNestedDict(data, "location");
            return GetValue<string>(location, "timezone", "Unknown");
        }

        /// <summary>
        /// Получить часовой пояс пользователя асинхронно
        /// Get user's timezone asynchronously
        /// </summary>
        public async Task<string> GetTimezoneAsync()
        {
            var data = await GetDataAsync();
            var location = GetNestedDict(data, "location");
            return GetValue<string>(location, "timezone", "Unknown");
        }

        /// <summary>
        /// Получить название провайдера (ISP)
        /// Get ISP (Internet Service Provider)
        /// </summary>
        public string GetIsp()
        {
            var data = GetData();
            var connection = GetNestedDict(data, "connection");
            return GetValue<string>(connection, "isp", "Unknown");
        }

        /// <summary>
        /// Получить название провайдера асинхронно
        /// Get ISP asynchronously
        /// </summary>
        public async Task<string> GetIspAsync()
        {
            var data = await GetDataAsync();
            var connection = GetNestedDict(data, "connection");
            return GetValue<string>(connection, "isp", "Unknown");
        }

        /// <summary>
        /// Получить организацию
        /// Get organization
        /// </summary>
        public string GetOrganization()
        {
            var data = GetData();
            var connection = GetNestedDict(data, "connection");
            return GetValue<string>(connection, "organization", "Unknown");
        }

        /// <summary>
        /// Получить организацию асинхронно
        /// Get organization asynchronously
        /// </summary>
        public async Task<string> GetOrganizationAsync()
        {
            var data = await GetDataAsync();
            var connection = GetNestedDict(data, "connection");
            return GetValue<string>(connection, "organization", "Unknown");
        }

        /// <summary>
        /// Получить номер автономной системы (AS)
        /// Get Autonomous System number
        /// </summary>
        public string GetAsNumber()
        {
            var data = GetData();
            var connection = GetNestedDict(data, "connection");
            return GetValue<string>(connection, "as_number", "Unknown");
        }

        /// <summary>
        /// Получить номер автономной системы асинхронно
        /// Get Autonomous System number asynchronously
        /// </summary>
        public async Task<string> GetAsNumberAsync()
        {
            var data = await GetDataAsync();
            var connection = GetNestedDict(data, "connection");
            return GetValue<string>(connection, "as_number", "Unknown");
        }

        /// <summary>
        /// Проверить, является ли соединение мобильным
        /// Check if connection is mobile
        /// </summary>
        public bool IsMobileConnection()
        {
            var data = GetData();
            var connection = GetNestedDict(data, "connection");
            return GetValue<bool>(connection, "is_mobile", false);
        }

        /// <summary>
        /// Проверить, является ли соединение мобильным асинхронно
        /// Check if connection is mobile asynchronously
        /// </summary>
        public async Task<bool> IsMobileConnectionAsync()
        {
            var data = await GetDataAsync();
            var connection = GetNestedDict(data, "connection");
            return GetValue<bool>(connection, "is_mobile", false);
        }

        /// <summary>
        /// Проверить, используется ли прокси
        /// Check if proxy is used
        /// </summary>
        public bool IsProxy()
        {
            var data = GetData();
            var connection = GetNestedDict(data, "connection");
            return GetValue<bool>(connection, "is_proxy", false);
        }

        /// <summary>
        /// Проверить, используется ли прокси асинхронно
        /// Check if proxy is used asynchronously
        /// </summary>
        public async Task<bool> IsProxyAsync()
        {
            var data = await GetDataAsync();
            var connection = GetNestedDict(data, "connection");
            return GetValue<bool>(connection, "is_proxy", false);
        }

        /// <summary>
        /// Проверить, находится ли IP в хостинге
        /// Check if IP belongs to hosting
        /// </summary>
        public bool IsHosting()
        {
            var data = GetData();
            var connection = GetNestedDict(data, "connection");
            return GetValue<bool>(connection, "is_hosting", false);
        }

        /// <summary>
        /// Проверить, находится ли IP в хостинге асинхронно
        /// Check if IP belongs to hosting asynchronously
        /// </summary>
        public async Task<bool> IsHostingAsync()
        {
            var data = await GetDataAsync();
            var connection = GetNestedDict(data, "connection");
            return GetValue<bool>(connection, "is_hosting", false);
        }

        /// <summary>
        /// Получить тип устройства (Desktop/Mobile)
        /// Get device type (Desktop/Mobile)
        /// </summary>
        public string GetDeviceType()
        {
            var data = GetData();
            var device = GetNestedDict(data, "device");
            return GetValue<string>(device, "type", "Unknown");
        }

        /// <summary>
        /// Получить тип устройства асинхронно
        /// Get device type asynchronously
        /// </summary>
        public async Task<string> GetDeviceTypeAsync()
        {
            var data = await GetDataAsync();
            var device = GetNestedDict(data, "device");
            return GetValue<string>(device, "type", "Unknown");
        }

        /// <summary>
        /// Получить операционную систему
        /// Get operating system
        /// </summary>
        public string GetOperatingSystem()
        {
            var data = GetData();
            var device = GetNestedDict(data, "device");
            return GetValue<string>(device, "os", "Unknown");
        }

        /// <summary>
        /// Получить операционную систему асинхронно
        /// Get operating system asynchronously
        /// </summary>
        public async Task<string> GetOperatingSystemAsync()
        {
            var data = await GetDataAsync();
            var device = GetNestedDict(data, "device");
            return GetValue<string>(device, "os", "Unknown");
        }

        /// <summary>
        /// Получить браузер
        /// Get browser
        /// </summary>
        public string GetBrowser()
        {
            var data = GetData();
            var device = GetNestedDict(data, "device");
            return GetValue<string>(device, "browser", "Unknown");
        }

        /// <summary>
        /// Получить браузер асинхронно
        /// Get browser asynchronously
        /// </summary>
        public async Task<string> GetBrowserAsync()
        {
            var data = await GetDataAsync();
            var device = GetNestedDict(data, "device");
            return GetValue<string>(device, "browser", "Unknown");
        }

        /// <summary>
        /// Получить User-Agent
        /// Get User-Agent string
        /// </summary>
        public string GetUserAgent()
        {
            var data = GetData();
            var device = GetNestedDict(data, "device");
            return GetValue<string>(device, "user_agent", "Unknown");
        }

        /// <summary>
        /// Получить User-Agent асинхронно
        /// Get User-Agent string asynchronously
        /// </summary>
        public async Task<string> GetUserAgentAsync()
        {
            var data = await GetDataAsync();
            var device = GetNestedDict(data, "device");
            return GetValue<string>(device, "user_agent", "Unknown");
        }

        /// <summary>
        /// Получить метод запроса
        /// Get request method
        /// </summary>
        public string GetRequestMethod()
        {
            var data = GetData();
            var request = GetNestedDict(data, "request");
            return GetValue<string>(request, "method", "Unknown");
        }

        /// <summary>
        /// Получить протокол запроса
        /// Get request protocol
        /// </summary>
        public string GetRequestProtocol()
        {
            var data = GetData();
            var request = GetNestedDict(data, "request");
            return GetValue<string>(request, "protocol", "Unknown");
        }

        /// <summary>
        /// Получить временную метку запроса
        /// Get request timestamp
        /// </summary>
        public long GetTimestamp()
        {
            var data = GetData();
            var request = GetNestedDict(data, "request");
            return GetValue<long>(request, "timestamp", 0);
        }

        /// <summary>
        /// Получить дату и время запроса
        /// Get request date and time
        /// </summary>
        public string GetDateTime()
        {
            var data = GetData();
            var request = GetNestedDict(data, "request");
            return GetValue<string>(request, "datetime", "Unknown");
        }

        /// <summary>
        /// Получить список языков пользователя
        /// Get user's languages list
        /// </summary>
        public List<Dictionary<string, object>> GetLanguages()
        {
            var data = GetData();
            var client = GetNestedDict(data, "client");
            var languages = GetArray(client, "languages");

            var result = new List<Dictionary<string, object>>();
            foreach (var lang in languages)
            {
                if (lang is Dictionary<string, object> langDict)
                {
                    result.Add(langDict);
                }
            }
            return result;
        }

        /// <summary>
        /// Получить список языков пользователя асинхронно
        /// Get user's languages list asynchronously
        /// </summary>
        public async Task<List<Dictionary<string, object>>> GetLanguagesAsync()
        {
            var data = await GetDataAsync();
            var client = GetNestedDict(data, "client");
            var languages = GetArray(client, "languages");

            var result = new List<Dictionary<string, object>>();
            foreach (var lang in languages)
            {
                if (lang is Dictionary<string, object> langDict)
                {
                    result.Add(langDict);
                }
            }
            return result;
        }

        /// <summary>
        /// Получить основной язык пользователя
        /// Get user's primary language
        /// </summary>
        public string GetPrimaryLanguage()
        {
            var languages = GetLanguages();
            if (languages.Count > 0 && languages[0].ContainsKey("code"))
            {
                return languages[0]["code"]?.ToString() ?? "Unknown";
            }
            return "Unknown";
        }

        /// <summary>
        /// Получить Referer
        /// Get Referer
        /// </summary>
        public string GetReferer()
        {
            var data = GetData();
            var client = GetNestedDict(data, "client");
            return GetValue<string>(client, "referer", "");
        }

        /// <summary>
        /// Получить URI запроса
        /// Get request URI
        /// </summary>
        public string GetRequestUri()
        {
            var data = GetData();
            var client = GetNestedDict(data, "client");
            return GetValue<string>(client, "request_uri", "");
        }

        /// <summary>
        /// Получить версию PHP на сервере
        /// Get PHP version on server
        /// </summary>
        public string GetPhpVersion()
        {
            var data = GetData();
            var server = GetNestedDict(data, "server");
            return GetValue<string>(server, "php_version", "Unknown");
        }

        /// <summary>
        /// Получить все данные в виде Dictionary
        /// Get all data as Dictionary
        /// </summary>
        public Dictionary<string, object> GetAllData()
        {
            return GetData();
        }

        /// <summary>
        /// Получить все данные в виде Dictionary асинхронно
        /// Get all data as Dictionary asynchronously
        /// </summary>
        public async Task<Dictionary<string, object>> GetAllDataAsync()
        {
            return await GetDataAsync();
        }

        /// <summary>
        /// Получить сырой JSON
        /// Get raw JSON
        /// </summary>
        public string GetRawJson()
        {
            try
            {
                var response = _httpClient.GetAsync(_apiUrl).Result;
                response.EnsureSuccessStatusCode();
                return response.Content.ReadAsStringAsync().Result;
            }
            catch (Exception ex)
            {
                return $"{{\"success\":false,\"error\":\"{ex.Message}\"}}";
            }
        }

        /// <summary>
        /// Получить сырой JSON асинхронно
        /// Get raw JSON asynchronously
        /// </summary>
        public async Task<string> GetRawJsonAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync(_apiUrl);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                return $"{{\"success\":false,\"error\":\"{ex.Message}\"}}";
            }
        }

        /// <summary>
        /// Проверить доступность API
        /// Check API availability
        /// </summary>
        public bool IsApiAvailable()
        {
            try
            {
                var response = _httpClient.GetAsync(_apiUrl).Result;
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Очистить кэш
        /// Clear cache
        /// </summary>
        public void ClearCache()
        {
            _cachedData = null;
        }

        /// <summary>
        /// Обновить данные принудительно
        /// Force refresh data
        /// </summary>
        public void RefreshData()
        {
            GetData(true);
        }

        /// <summary>
        /// Обновить данные принудительно асинхронно
        /// Force refresh data asynchronously
        /// </summary>
        public async Task RefreshDataAsync()
        {
            await GetDataAsync(true);
        }

        #endregion

        #region Комбинированные методы / Combined methods

        /// <summary>
        /// Получить основную информацию (IP, страна, город)
        /// Get basic information (IP, country, city)
        /// </summary>
        public string GetBasicInfo()
        {
            return $"IP: {GetIpAddress()}\n" +
                   $"Страна / Country: {GetCountry()}\n" +
                   $"Город / City: {GetCity()}";
        }

        /// <summary>
        /// Получить информацию о соединении
        /// Get connection information
        /// </summary>
        public string GetConnectionInfo()
        {
            return $"Провайдер / ISP: {GetIsp()}\n" +
                   $"Организация / Organization: {GetOrganization()}\n" +
                   $"AS: {GetAsNumber()}\n" +
                   $"Мобильное / Mobile: {(IsMobileConnection() ? "Да / Yes" : "Нет / No")}\n" +
                   $"Прокси / Proxy: {(IsProxy() ? "Да / Yes" : "Нет / No")}\n" +
                   $"Хостинг / Hosting: {(IsHosting() ? "Да / Yes" : "Нет / No")}";
        }

        /// <summary>
        /// Получить информацию об устройстве
        /// Get device information
        /// </summary>
        public string GetDeviceInfo()
        {
            return $"Тип / Type: {GetDeviceType()}\n" +
                   $"ОС / OS: {GetOperatingSystem()}\n" +
                   $"Браузер / Browser: {GetBrowser()}";
        }

        /// <summary>
        /// Получить полную информацию о пользователе
        /// Get complete user information
        /// </summary>
        public string GetAllInfo()
        {
            var sb = new StringBuilder();

            sb.AppendLine("=== ИНФОРМАЦИЯ О ПОЛЬЗОВАТЕЛЕ / USER INFORMATION ===");
            sb.AppendLine();

            sb.AppendLine("МЕСТОПОЛОЖЕНИЕ / LOCATION:");
            sb.AppendLine($"  IP: {GetIpAddress()}");
            sb.AppendLine($"  Страна / Country: {GetCountry()}");
            sb.AppendLine($"  Город / City: {GetCity()}");
            sb.AppendLine($"  Регион / Region: {GetRegion()}");
            sb.AppendLine($"  Часовой пояс / Timezone: {GetTimezone()}");
            sb.AppendLine();

            sb.AppendLine("СОЕДИНЕНИЕ / CONNECTION:");
            sb.AppendLine($"  Провайдер / ISP: {GetIsp()}");
            sb.AppendLine($"  Организация / Organization: {GetOrganization()}");
            sb.AppendLine($"  AS: {GetAsNumber()}");
            sb.AppendLine($"  Мобильное / Mobile: {(IsMobileConnection() ? "Да / Yes" : "Нет / No")}");
            sb.AppendLine($"  Прокси / Proxy: {(IsProxy() ? "Да / Yes" : "Нет / No")}");
            sb.AppendLine($"  Хостинг / Hosting: {(IsHosting() ? "Да / Yes" : "Нет / No")}");
            sb.AppendLine();

            sb.AppendLine("УСТРОЙСТВО / DEVICE:");
            sb.AppendLine($"  Тип / Type: {GetDeviceType()}");
            sb.AppendLine($"  ОС / OS: {GetOperatingSystem()}");
            sb.AppendLine($"  Браузер / Browser: {GetBrowser()}");
            sb.AppendLine();

            sb.AppendLine("ЯЗЫКИ / LANGUAGES:");
            var languages = GetLanguages();
            foreach (var lang in languages)
            {
                string code = lang.ContainsKey("code") ? lang["code"]?.ToString() : "Unknown";
                string quality = lang.ContainsKey("quality") ? lang["quality"]?.ToString() : "1.0";
                sb.AppendLine($"  {code} (quality: {quality})");
            }

            sb.AppendLine();
            sb.AppendLine($"Версия PHP на сервере / PHP version on server: {GetPhpVersion()}");

            return sb.ToString();
        }

        #endregion

        #region IDisposable implementation

        /// <summary>
        /// Освободить ресурсы
        /// Dispose resources
        /// </summary>
        public void Dispose()
        {
            _httpClient?.Dispose();
        }

        #endregion
    }
}