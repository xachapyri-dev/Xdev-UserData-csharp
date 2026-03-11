# Xdev.UserData

<div align="center">
  <h3>Библиотека для получения информации о пользователе через PHP API</h3>
  
  <p>
    <a href="https://www.nuget.org/packages/Xdev.UserData/">
      <img src="https://img.shields.io/nuget/v/Xdev.UserData.svg" alt="NuGet Version">
    </a>
    <a href="https://www.nuget.org/packages/Xdev.UserData/">
      <img src="https://img.shields.io/nuget/dt/Xdev.UserData.svg" alt="NuGet Downloads">
    </a>
    <a href="https://github.com/xachapyri-dev/Xdev-UserData-csharp/blob/main/LICENSE">
      <img src="https://img.shields.io/badge/License-MIT-yellow.svg" alt="License: MIT">
    </a>
    <a href="https://dotnet.microsoft.com/">
      <img src="https://img.shields.io/badge/.NET-4.6.1%20|%20.NET%20Core%203.1%20|%20.NET%205%2B%20|%20.NET%20Standard%202.0%2B-blue" alt=".NET">
    </a>
  </p>
</div>

---

##Содержание

- [Описание](#описание)
- [ВАЖНОЕ ПРЕДУПРЕЖДЕНИЕ](#важное-предупреждение)
- [Возможности](#возможности)
- [Поддерживаемые платформы](#поддерживаемые-платформы)
- [Установка](#установка)
- [Быстрый старт](#быстрый-старт)
- [Примеры использования](#примеры-использования)
- [API методы](#api-методы)
- [Структура данных](#структура-данных)
- [Требования к серверу](#требования-к-серверу)
- [Установка API на ваш сервер](#установка-api-на-ваш-сервер)
- [Отказ от ответственности](#отказ-от-ответственности)

---

## Описание

**Xdev.UserData** — это мощная и простая в использовании библиотека на C# для получения подробной информации о пользователе через PHP API. Она позволяет получать данные об IP-адресе, географическом местоположении, интернет-провайдере, устройстве, браузере и многом другом.

## ВАЖНОЕ ПРЕДУПРЕЖДЕНИЕ

**Для определения геолокации, провайдера и других данных используется стороннее API:**  
`http://ip-api.com/json/`

Это бесплатный сервис с ограничением **45 запросов в минуту**.  
Ваш IP-адрес передается на сторонний сервер для определения:
-  Страны (country)
-  Города (city)
-  Региона (region)
-  Часового пояса (timezone)
-  Провайдера (ISP)
-  Организации (org)
-  Номера AS (as)
-  Типа соединения (mobile/proxy/hosting)

Политика конфиденциальности: https://ip-api.com/docs/legal

## Возможности

- ✅ **Получение IP-адреса** пользователя
- ✅ **Геолокация**: страна, город, регион, часовой пояс
- ✅ **Информация о провайдере**: ISP, организация, номер AS
- ✅ **Данные об устройстве**: тип (Desktop/Mobile), ОС, браузер, User-Agent
- ✅ **Информация о запросе**: метод, протокол, временная метка
- ✅ **Языковые предпочтения** пользователя
- ✅ **Проверка**: мобильное соединение, прокси, хостинг
- ✅ **Синхронные и асинхронные** методы
- ✅ **Встроенное кэширование** данных (30 секунд)
- ✅ **Безопасное приведение типов**
- ✅ **Поддержка всех основных платформ** .NET

## Поддерживаемые платформы

<div align="center">
  
  | Платформа | Версии |
  |-----------|--------|
  | .NET Framework | 4.6.1, 4.6.2, 4.7, 4.7.1, 4.7.2, 4.8, 4.8.1 |
  | .NET Core | 3.1 |
  | .NET | 5.0, 6.0, 7.0, 8.0, 9.0, 10.0 |
  | .NET Standard | 2.0, 2.1 |

</div>

## Установка

<div align="center">
  
  | NuGet | Репозиторий |
  |-------|-------------|
  | ``` dotnet add package Xdev.UserData ``` | [Latest](https://github.com/xachapyri-dev/Xdev-UserData-csharp/releases/latest) |

</div>

## Быстрый старт
``` csharp
using Xdev.UserData;

// Создаем экземпляр клиента
using (var userInfo = new UserInfo())
{
    // Получаем отдельные поля
    string ip = userInfo.GetIpAddress();
    string country = userInfo.GetCountry();
    string city = userInfo.GetCity();
    string browser = userInfo.GetBrowser();
    
    Console.WriteLine($"IP: {ip}");
    Console.WriteLine($"Страна: {country}");
    Console.WriteLine($"Город: {city}");
    Console.WriteLine($"Браузер: {browser}");
    
    // Получаем полную информацию
    Console.WriteLine(userInfo.GetAllInfo());
}
```
## Примеры использования

## Пример 1: Получение основной информации
``` csharp
using Xdev.UserData;

using (var userInfo = new UserInfo())
{
    string basicInfo = userInfo.GetBasicInfo();
    Console.WriteLine(basicInfo);
}
```
* Вывод
```
IP: your IP
Страна: Russia
Город: Kirov
```

## Пример 2: Асинхронное получение данных
``` csharp
using Xdev.UserData;

using (var userInfo = new UserInfo())
{
    // Асинхронное получение
    string country = await userInfo.GetCountryAsync();
    string city = await userInfo.GetCityAsync();
    bool isMobile = await userInfo.IsMobileConnectionAsync();
    
    Console.WriteLine($"Страна: {country}");
    Console.WriteLine($"Город: {city}");
    Console.WriteLine($"Мобильное: {isMobile}");
}
```

## Пример 3: Работа с языками
``` csharp
using Xdev.UserData;

using (var userInfo = new UserInfo())
{
    var languages = userInfo.GetLanguages();
    
    Console.WriteLine("Языки пользователя:");
    foreach (var lang in languages)
    {
        string code = lang["code"]?.ToString();
        double quality = Convert.ToDouble(lang["quality"]);
        Console.WriteLine($"  {code} (приоритет: {quality})");
    }
    
    string primaryLang = userInfo.GetPrimaryLanguage();
    Console.WriteLine($"Основной язык: {primaryLang}");
}
```

## Пример 4: Использование своего API
``` csharp
using Xdev.UserData;

// Свой сервер с PHP скриптом
using (var userInfo = new UserInfo("https://ваш-сайт.ru/api/user-data.php"))
{
    var data = userInfo.GetAllData();
}
```
> [!NOTE]
> О том как установить API на свой сервер будет рассказано в [Установка API на ваш сервер](#установка-api-на-ваш-сервер)

## Пример 5: Сохранение в файл
``` csharp
using Xdev.UserData;

using (var userInfo = new UserInfo())
{
    // Сохраняем JSON в файл
    string json = userInfo.GetRawJson();
    System.IO.File.WriteAllText("user_data.json", json);
    
    // Асинхронное сохранение
    await userInfo.SaveToFileAsync("user_data_pretty.json", true);
}
```

## Пример 6: Полная информация

``` csharp
using Xdev.UserData;

using (var userInfo = new UserInfo())
{
    Console.WriteLine(userInfo.GetAllInfo());
}
```
* Вывод
```
=== ИНФОРМАЦИЯ О ПОЛЬЗОВАТЕЛЕ ===

МЕСТОПОЛОЖЕНИЕ:
  IP: your IP
  Страна: Russia
  Город: Kirov
  Регион: Kaluga Oblast
  Часовой пояс: Europe/Moscow

СОЕДИНЕНИЕ:
  Провайдер: название вашего провайдера
  Организация: организация вашего провайдера
  AS: (хех)
  Мобильное: Да
  Прокси: Нет
  Хостинг: Нет

УСТРОЙСТВО:
  Тип: Desktop
  ОС: Windows
  Браузер: Edge

ЯЗЫКИ:
  ru (quality: 1)
  en (quality: 0.9)

Версия PHP на сервере: 5.6.40
```
> [!WARNING]
> Выводимые данные уникальны каждому

## API Методы

## Основные данные

<div align="center">
  
  | Метод/Асинхронный метод | Описание | Источник |
  |-------------------------|----------|----------|
  | ```GetIpAddress()``` / ```GetIpAddressAsync()``` | 	Получить IP-адрес | Локально |
  | ```GetCountry()``` / ```GetCountryAsync()``` | 	Получить название страны | ip-api.com |
  | ```GetCity()```/ ```GetCityAsync()``` | Получить название города | ip-api.com |
  | ```GetRegion()``` / ```GetRegionAsync()``` | 	Получить название региона | ip-api.com |
  | ```GetTimezone()``` / ```GetTimezoneAsync()``` | 	Получить часовой пояс | ip-api.com |
  
</div>

## Данные о соединении

<div align="center">
  
  | Метод/Асинхронный метод | Описание | Источник |
  |-------------------------|----------|----------|
  | ```GetIsp()``` / ```GetIspAsync()``` | Получить название провайдера | ip-api.com |
  | ```GetOrganization()``` / ```GetOrganizationAsync()``` | Получить название организации | ip-api.com |
  | ```GetAsNumber()``` / ```GetAsNumberAsync()``` | Получить номер AS | ip-api.com |
  | ```IsMobileConnection()``` / ```IsMobileConnectionAsync()``` | Проверить мобильное соединение | ip-api.com |
  | ```IsHosting()``` / ```IsHostingAsync()``` | Проверить хостинг | ip-api.com |
  | ```IsProxy()``` / ```IsProxyAsync()``` | Проверить прокси | ip-api.com |
  
</div>

## Данные об устройстве

<div align="center">
  
  | Метод/Асинхронный метод | Описание | Источник |
  |-------------------------|----------|----------|
  | ```GetDeviceType()``` / ```GetDeviceTypeAsync()``` | Получить тип устройства | Локально |
  | ```GetOperatingSystem()``` / ```GetOperatingSystemAsync()``` | Получить ОС | Локально |
  | ```GetBrowser()``` / ```GetBrowserAsync()``` | Получить браузер | Локально |
  | ```GetUserAgent()``` / ```GetUserAgentAsync()``` | Получить User-Agent | Локально |
  
</div>

## Данные запроса

<div align="center">
  
  | Метод/Асинхронный метод | Описание |
  |-------------------------|----------|
  | ```GetRequestMethod()``` | Получить метод запроса |
  | ```GetRequestProtocol()``` | Получить протокол |
  | ```GetTimestamp()``` | Получить временную метку |
  | ```GetDateTime()``` |	Получить дату и время |
  
</div>

## Языки

<div align="center">
  
  | Метод/Асинхронный метод | Описание |
  |-------------------------|----------|
  | ```GetLanguages()``` / ```GetLanguagesAsync()``` | Получить список языков |
  | ```GetPrimaryLanguage()``` | Получить основной язык |
  
</div>

## Прочие методы

<div align="center">
  
  | Метод/Асинхронный метод | Описание |
  |-------------------------|----------|
  | ```GetReferer()``` | Получить Referer |
  | ```GetRequestUri()``` |	Получить URI запроса |
  | ```GetPhpVersion()``` |	Получить версию PHP |
  | ```GetAllData()``` / ```GetAllDataAsync()``` | Получить все данные |
  | ```GetRawJson()``` / ```GetRawJsonAsync()``` | Получить сырой JSON |
  | ```IsApiAvailable()``` | Проверить доступность API |
  | ```ClearCache()``` | Очистить кэш |
  | ```RefreshData()``` / ```RefreshDataAsync()``` | Обновить данные |
  
</div>

## Комбинированные методы

<div align="center">
  
  | Метод/Асинхронный метод | Описание |
  |-------------------------|----------|
  | ```GetBasicInfo()``` | Основная информация |
  | ```GetConnectionInfo()``` |	Информация о соединении |
  | ```GetDeviceInfo()```	| Информация об устройстве |
  | ```GetAllInfo()``` | Полная информация |
  
</div>

## Структура данных

``` json

{
  "success": true,
  "data": {
    "ip": "your ip",
    "location": {
      "country": "Russia",
      "city": "Kirov",
      "region": "Kaluga Oblast",
      "timezone": "Europe/Moscow"
    },
    "connection": {
      "isp": "your isp",
      "organization": "your organization",
      "as_number": "your as_number",
      "is_mobile": true,
      "is_proxy": false,
      "is_hosting": false
    },
    "device": {
      "type": "Desktop",
      "os": "Windows",
      "browser": "Edge",
      "user_agent": "Mozilla/5.0..."
    },
    "request": {
      "method": "GET",
      "protocol": "HTTP/1.0",
      "timestamp": "your timestamp",
      "datetime": "2026-03-11 15:32:12"
    },
    "client": {
      "languages": [
        {"code": "ru", "quality": 1},
        {"code": "en", "quality": 0.9}
      ],
      "referer": "",
      "request_uri": "/API/user_data.php"
    },
    "server": {
      "php_version": "5.6.40"
    }
  }
}
```
> [!WARNING]
> Выводимые данные уникальны каждому

## Требования к серверу

По умолчанию используется публичное API:

```text
http://k90052gj.beget.tech/API/user_data.php
```

Можно указать свой эндпоинт:

```csharp
var userInfo = new UserInfo("https://ваш-сайт.ru/api/user-data.php");
```

## Установка API на ваш сервер

> [!WARNING]
> Главное требованеие!!! Сервер должен поддерживать PHP версии 5.6!!!

* Для начало мы создаём файл с расширением ```.php```
* Потом мы вставляем этот код:
``` php
<?php

/**
 * ===================================================
 * ВАЖНОЕ ПРЕДУПРЕЖДЕНИЕ / IMPORTANT NOTICE
 * ===================================================
 * 
 * 🇷🇺 ДЛЯ ОПРЕДЕЛЕНИЯ ГЕОЛОКАЦИИ ИСПОЛЬЗУЕТСЯ СТОРОННЕЕ API:
 *    http://ip-api.com/json/
 * 
 *    Это бесплатный сервис с ограничением 45 запросов в минуту.
 *    Ваш IP-адрес передается на сторонний сервер для определения:
 *    - Страны (country)
 *    - Города (city)
 *    - Региона (region)
 *    - Часового пояса (timezone)
 *    - Провайдера (ISP)
 *    - Организации (org)
 *    - Номера AS (as)
 *    - Типа соединения (mobile/proxy/hosting)
 * 
 *    Политика конфиденциальности: https://ip-api.com/docs/legal
 * 
 * 🇬🇧 FOR GEOLOCATION DETECTION USES THIRD-PARTY API:
 *    http://ip-api.com/json/
 * 
 *    This is a free service with a limit of 45 requests per minute.
 *    Your IP address is sent to a third-party server to determine:
 *    - Country
 *    - City
 *    - Region
 *    - Timezone
 *    - ISP
 *    - Organization
 *    - AS number
 *    - Connection type (mobile/proxy/hosting)
 * 
 *    Privacy policy: https://ip-api.com/docs/legal
 * ===================================================
 */

/**
* MIT License

* Copyright (c) 2026 Хачапури dev

* Permission is hereby granted, free of charge, to any person obtaining a copy
* of this software and associated documentation files (the "Software"), to deal
* in the Software without restriction, including without limitation the rights
* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
* copies of the Software, and to permit persons to whom the Software is
* furnished to do so, subject to the following conditions:
* 
* The above copyright notice and this permission notice shall be included in all
* copies or substantial portions of the Software.
* 
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
* SOFTWARE.
*/
/**
 * GitHub: https://github.com/xachapyri-dev/Xdev-UserData-csharp
 */

header('Content-Type: application/json; charset=utf-8');
header('Access-Control-Allow-Origin: *');
header('Access-Control-Allow-Methods: GET');
header('X-Content-Type-Options: nosniff');

// Добавляем предупреждение в JSON ответ
$warning = [
    'notice' => [
        'ru' => 'Для определения геолокации используется стороннее API ip-api.com. Ваш IP передается на сторонний сервер.',
        'en' => 'Third-party API ip-api.com is used for geolocation. Your IP is sent to a third-party server.',
        'limit' => '45 requests per minute',
        'privacy' => 'https://ip-api.com/docs/legal'
    ]
];

/**
 * Получить IP-адрес пользователя
 */
function getUserIP() {
    $server = $_SERVER;
    
    $headers = array(
        'HTTP_CLIENT_IP',
        'HTTP_X_FORWARDED_FOR',
        'HTTP_X_FORWARDED',
        'HTTP_FORWARDED_FOR',
        'HTTP_FORWARDED',
        'REMOTE_ADDR'
    );
    
    foreach ($headers as $header) {
        if (isset($server[$header])) {
            $ip = $server[$header];
            
            if (strpos($ip, ',') !== false) {
                $ips = explode(',', $ip);
                $ip = trim($ips[0]);
            }
            
            if (filter_var($ip, FILTER_VALIDATE_IP)) {
                return $ip;
            }
        }
    }
    
    return 'UNKNOWN';
}

/**
 * Получить геоданные по IP через ip-api.com
 */
function getLocationFromIP($ip) {
    // Исключаем локальные IP
    $localIPs = array('127.0.0.1', '::1', 'localhost');
    if (in_array($ip, $localIPs) || $ip == 'UNKNOWN') {
        return array(
            'country' => 'Localhost',
            'city' => 'Localhost',
            'region' => 'Localhost',
            'timezone' => 'UTC',
            'isp' => 'Localhost',
            'org' => 'Localhost',
            'as' => 'Localhost',
            'is_mobile' => false,
            'is_proxy' => false,
            'is_hosting' => false,
            'source' => 'local'
        );
    }
    
    // Используем API ip-api.com
    $url = "http://ip-api.com/json/{$ip}?fields=status,country,city,regionName,timezone,isp,org,as,mobile,proxy,hosting";
    
    $context = stream_context_create(array(
        'http' => array(
            'method' => 'GET',
            'timeout' => 3,
            'header' => "User-Agent: Xdev.UserData/1.0\r\n"
        )
    ));
    
    $response = @file_get_contents($url, false, $context);
    
    if ($response !== false) {
        $data = json_decode($response, true);
        
        if ($data && isset($data['status']) && $data['status'] == 'success') {
            return array(
                'country' => isset($data['country']) ? $data['country'] : 'Unknown',
                'city' => isset($data['city']) ? $data['city'] : 'Unknown',
                'region' => isset($data['regionName']) ? $data['regionName'] : 'Unknown',
                'timezone' => isset($data['timezone']) ? $data['timezone'] : 'Unknown',
                'isp' => isset($data['isp']) ? $data['isp'] : 'Unknown',
                'org' => isset($data['org']) ? $data['org'] : 'Unknown',
                'as' => isset($data['as']) ? $data['as'] : 'Unknown',
                'is_mobile' => isset($data['mobile']) ? (bool)$data['mobile'] : false,
                'is_proxy' => isset($data['proxy']) ? (bool)$data['proxy'] : false,
                'is_hosting' => isset($data['hosting']) ? (bool)$data['hosting'] : false,
                'source' => 'ip-api.com'
            );
        }
    }
    
    // Если API не отвечает, возвращаем неизвестные данные
    return array(
        'country' => 'Unknown',
        'city' => 'Unknown',
        'region' => 'Unknown',
        'timezone' => 'Unknown',
        'isp' => 'Unknown',
        'org' => 'Unknown',
        'as' => 'Unknown',
        'is_mobile' => false,
        'is_proxy' => false,
        'is_hosting' => false,
        'source' => 'failed'
    );
}

/**
 * Получить информацию об устройстве
 */
function getDeviceInfo($userAgent) {
    $browser = 'Unknown';
    $os = 'Unknown';
    $device = 'Desktop';
    
    if (strpos($userAgent, 'Firefox') !== false) $browser = 'Firefox';
    elseif (strpos($userAgent, 'Chrome') !== false) $browser = 'Chrome';
    elseif (strpos($userAgent, 'Safari') !== false) $browser = 'Safari';
    elseif (strpos($userAgent, 'Opera') !== false || strpos($userAgent, 'OPR') !== false) $browser = 'Opera';
    elseif (strpos($userAgent, 'Edg') !== false) $browser = 'Edge';
    elseif (strpos($userAgent, 'MSIE') !== false || strpos($userAgent, 'Trident') !== false) $browser = 'Internet Explorer';
    
    if (strpos($userAgent, 'Windows NT') !== false) $os = 'Windows';
    elseif (strpos($userAgent, 'Mac OS X') !== false) $os = 'macOS';
    elseif (strpos($userAgent, 'Linux') !== false) $os = 'Linux';
    elseif (strpos($userAgent, 'Android') !== false) $os = 'Android';
    elseif (strpos($userAgent, 'iPhone') !== false || strpos($userAgent, 'iPad') !== false) $os = 'iOS';
    
    if (strpos($userAgent, 'Mobile') !== false || strpos($userAgent, 'Android') !== false || 
        strpos($userAgent, 'iPhone') !== false) $device = 'Mobile';
    
    return array(
        'browser' => $browser,
        'os' => $os,
        'device' => $device,
        'user_agent' => $userAgent
    );
}

/**
 * Получить языковые предпочтения
 */
function getLanguages($acceptLang) {
    $languages = array();
    
    if (!empty($acceptLang)) {
        $langs = explode(',', $acceptLang);
        foreach ($langs as $lang) {
            $parts = explode(';q=', $lang);
            $code = trim($parts[0]);
            $quality = isset($parts[1]) ? (float)$parts[1] : 1.0;
            
            $languages[] = array(
                'code' => $code,
                'quality' => $quality
            );
        }
        
        usort($languages, function($a, $b) {
            if ($a['quality'] == $b['quality']) return 0;
            return ($a['quality'] < $b['quality']) ? 1 : -1;
        });
    }
    
    return $languages;
}

// Основной код
$userIP = getUserIP();
$userAgent = isset($_SERVER['HTTP_USER_AGENT']) ? $_SERVER['HTTP_USER_AGENT'] : '';
$acceptLang = isset($_SERVER['HTTP_ACCEPT_LANGUAGE']) ? $_SERVER['HTTP_ACCEPT_LANGUAGE'] : '';

// Получаем геоданные через стороннее API
$location = getLocationFromIP($userIP);

// Получаем информацию об устройстве
$deviceInfo = getDeviceInfo($userAgent);

// Получаем языки
$languages = getLanguages($acceptLang);

// Собираем все данные
$userData = array(
    'success' => true,
    'notice' => $warning['notice'], // Добавляем предупреждение в ответ
    'data' => array(
        'ip' => $userIP,
        'data_source' => $location['source'], // Указываем источник данных
        'location' => array(
            'country' => $location['country'],
            'city' => $location['city'],
            'region' => $location['region'],
            'timezone' => $location['timezone'],
            'latitude' => null, // API не возвращает координаты в этом запросе
            'longitude' => null
        ),
        'connection' => array(
            'isp' => $location['isp'],
            'organization' => $location['org'],
            'as_number' => $location['as'],
            'is_mobile' => $location['is_mobile'],
            'is_proxy' => $location['is_proxy'],
            'is_hosting' => $location['is_hosting']
        ),
        'device' => array(
            'type' => $deviceInfo['device'],
            'os' => $deviceInfo['os'],
            'browser' => $deviceInfo['browser'],
            'user_agent' => $deviceInfo['user_agent']
        ),
        'request' => array(
            'method' => isset($_SERVER['REQUEST_METHOD']) ? $_SERVER['REQUEST_METHOD'] : 'GET',
            'protocol' => isset($_SERVER['SERVER_PROTOCOL']) ? $_SERVER['SERVER_PROTOCOL'] : 'HTTP/1.1',
            'timestamp' => time(),
            'datetime' => date('Y-m-d H:i:s')
        ),
        'client' => array(
            'languages' => $languages,
            'referer' => isset($_SERVER['HTTP_REFERER']) ? $_SERVER['HTTP_REFERER'] : '',
            'request_uri' => isset($_SERVER['REQUEST_URI']) ? $_SERVER['REQUEST_URI'] : ''
        ),
        'server' => array(
            'php_version' => PHP_VERSION
        )
    )
);

// Добавляем pretty print если запрошено
$options = JSON_UNESCAPED_UNICODE;
if (isset($_GET['pretty'])) {
    $options |= JSON_PRETTY_PRINT;
}

echo json_encode($userData, $options);
?>
```
* Загружаем на любой хостинг (сервер должен поддерживать PHP версии 5.6)
* Всё!
Используйте в вашем коде "[Требования к серверу](#требования-к-серверу)"

## Отказ от ответственности

**1. Использование стороннего API**  
Данная библиотека и соответствующий PHP-скрипт используют стороннее API `ip-api.com` для определения геолокации, информации о провайдере и других данных. Это означает, что:

- IP-адрес пользователя передается на сервер `ip-api.com`
- Сервер `ip-api.com` может собирать, хранить и обрабатывать эти данные в соответствии со своей политикой конфиденциальности
- Мы не контролируем и не несем ответственности за действия `ip-api.com`

**2. Ограничения бесплатного тарифа**  
Бесплатная версия `ip-api.com` имеет ограничение **45 запросов в минуту**. При превышении этого лимита:

- API может временно блокировать запросы
- Данные могут возвращаться с ошибкой или отсутствовать
- Рекомендуется использовать платную версию для высоконагруженных проектов

**3. Точность данных**  
Мы не гарантируем 100% точность предоставляемых данных, так как они получены от стороннего API. Геолокация по IP всегда является приблизительной и может содержать ошибки.

**4. Конфиденциальность пользователей**  
Разработчики, использующие эту библиотеку, обязаны:

- Уведомлять пользователей о сборе и передаче их IP-адресов
- Получать согласие пользователей, если это требуется по законодательству их страны
- Обеспечивать соблюдение требований GDPR, CCPA и других законов о конфиденциальности

**5. Ответственность**  
Авторы библиотеки не несут ответственности за:

- Недоступность или некорректную работу стороннего API
- Нарушение законодательства о конфиденциальности разработчиками, использующими эту библиотеку
- Ущерб, прямой или косвенный, связанный с использованием данной библиотеки
