# Приложение за скъсяване на URL адреси

ASP.NET Core MVC приложение за скъсяване на URL адреси. Това приложение позволява на потребителите да създават кратки URL адреси, да се регистрират и влизат, както и да управляват своите скъсени URL адреси. Приложението използва Entity Framework за съхранение на данни.

## Предварителни изисквания

Уверете се, че имате инсталиран **Entity Framework 6**, за да работите с приложението.

### Команди за настройка

В **Package Manager Console** в Visual Studio изпълнете следните команди, за да настроите базата данни:

```powershell
Enable-Migrations
Add-Migration <ИмеНаМиграция>
Update-Database
```

### Структура на проекта
Ето кратко описание на структурата на проекта:

## Controllers (Контролери)
HomeController.cs: Управлява основната страница на приложението.

UrlController.cs: Управлява действията, свързани с URL адресите, като създаване и управление на скъсени URL адреси.

UserController.cs: Управлява регистрацията, влизането и профилните действия на потребителите.

## Entity (Модели)
ShortenedURL.cs: Модел за скъсен URL адрес, включващ свойства като оригинален и скъсен URL адрес.

User.cs: Модел за потребител с необходимите свойства за един потребител, като име и парола.

## Models (Модели)
ErrorViewModel.cs: Модел, използван за обработка на грешки във визуализациите.

## Repository (Репозитории)
IDGenerator.cs: Генерира уникални ID за скъсените URL адреси.

UserRepository.cs: Управлява операции с данни, свързани с потребителите.

## SQLConnection (Връзка с базата данни)
Context.cs: Контекст за базата данни на Entity Framework, свързващ моделите с базата данни.

## ViewModel (Модели за визуализации)
Съдържа модели за предаване на данни към визуализациите:

MainUserVM.cs: Основен модел за данни, свързани с потребителите.
ShortenVM.cs: Модел за създаване и визуализация на скъсени URL адреси.

## Views (Визуализации)
Съдържа Razor визуализациите за потребителския интерфейс на приложението.

Home

Index.cshtml: Основна страница на приложението.
Shared

_Layout.cshtml: Основният шаблон, използван в приложението.
_ValidationScriptsPartial.cshtml: Скриптове за валидиране на клиентската страна.
Error.cshtml: Страница за грешки.
Url

DashBoard.cshtml: Визуализация за управление на потребителските URL адреси.
Delete.cshtml: Визуализация за изтриване на URL адрес.
Details.cshtml: Визуализация за детайли на URL адрес.
Edit.cshtml: Визуализация за редактиране на скъсен URL адрес.
ShortenUrl.cshtml: Визуализация за създаване на нов скъсен URL адрес.
User

Login.cshtml: Страница за вход на потребители.
Register.cshtml: Страница за регистрация на потребители.
_ViewImports.cshtml: Импорт файлове за визуализациите на потребителя.
_ViewStart.cshtml: Начални настройки за визуализациите.

## Program.cs
Главната входна точка на приложението.

### Инсталация
Клонирайте репозиторито.

Отворете решението в Visual Studio.

В Package Manager Console изпълнете следните команди за настройка на базата данни:
```powershell
Enable-Migrations
Add-Migration <ИмеНаМиграция>
Update-Database
```

И накрая Стартирайте приложението с ISS Express, тъй като са използвани бисквитки.

Приятно ползване на приложението за скъсяване на URL адреси!
