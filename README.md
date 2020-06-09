# AdminPanel
Небольшой сайт с http-сервисом для управления списками баз для пользователей домена (доступна загрузка списка пользователей из каталога AD). Резализует интерфейс сервиса (WebCommonInfoBases), описанный на ИТС. При корректной настройке, пользователь 1С при запуске 1cestart.exe будет получать актуальный список баз.

<h2>How to use:</h2>

В качестве базы данных сайт использует MSSQL. Для настройки сайта необходимо изменить файл appsettings.production.json, 
пример содержимого файла:

``` json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "Default": "Data Source=localhost;Initial Catalog=AdminPanel;Integrated Security=True;"
  },
  "Security": {
    "AdminGroup": "ENTERPRISE\\Администраторы 1С"
  },
  "WebDistributiveLocation": {
    "Catalog": "",
    "x64tox86": false
  }
}
```

Доступ к панелям управлениями пользователями, спискам баз и базам ограничивается группой домена. Для назначения администраторов 
необходимо создать группу безопасности AD и вписать ее имя в параметр AdminGroup секции Security в формате {DOMAIN_NAME}\\{GROUP_NAME}
и добавить в эту группу пользователей, имеющих доступ к веб-интерфейсу сайта.

Для получения списка баз в настройках проверки подлинности сайта IIS необходимо включить "Проверку подлинности Windows",
т.к. сопоставление пользователя для получения списка баз выполняется по SID.

Для начала использования http-сервиса WebCommonInfoBases необходимо в файле 1cestart.cfg заполнить параметр InternetService 
или WebDistributiveLocation, указав в значении адрес сайта.

![Alt text](/Users.png)
![Alt text](/InfoBasesLists.png)
![Alt text](/InfoBases.png)
