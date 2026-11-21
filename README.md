# LibraryDesk

Консольна система обліку книжкових видач для самостійної роботи 3.

## Дані роботи

Варіант 2. Анюхіна Анастасія, група 471.

## Швидкий запуск

Потрібен .NET 9 SDK.

```powershell
dotnet build
dotnet test
dotnet run --project LibraryDesk.App -- seed
dotnet run --project LibraryDesk.App
```

## Структура

- `LibraryDesk.Core` — домен, політики, сервіси, звіти й сховище.
- `LibraryDesk.App` — консольне меню та композиційний корінь.
- `LibraryDesk.Tests` — автоматизовані тести.
- `labs/LibraryDesk.Labs` — попередні лабораторні реалізації.

## Сценарії

Підтримуються видача книжок, повернення з розрахунком пені, скасування бронювання, пошук за читачем та ISBN, звіт боржників на дату.

## Тести й покриття

```powershell
dotnet test
dotnet test -c Release /p:CollectCoverage=true
```

## Технології

.NET 9, C#, xUnit, coverlet.msbuild, Roslyn Banned API Analyzer, StyleCop, GitHub Actions.

## Обмеження

Застосунок консольний. Дані зберігаються локально у JSON. Онлайн-сервіси, банківські платежі та графічний інтерфейс не входять до меж системи.