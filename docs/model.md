# Класова модель LibraryDesk (варіант 2)

## Сутності

| Сутність | Роль | Незмінний | Головний інваріант |
|---|---|---|---|
| LoanItem | позиція документа | так, record | Days > 0, DailyRate >= 0, Isbn не порожній |
| Reader | довідник учасників | так, record | Id > 0, Name не порожнє, Email містить @ |
| Loan | документ зі станом | ні, агрегат | Id > 0, ReaderId > 0; позиції додають лише у стані Reserved |
| Book | довідник об'єктів обліку | ні (CopiesAvailable змінюється) | CopiesAvailable >= 0, DailyRate >= 0 |
| ArchivedLoan | перегляд завершеної видачі | так (лише читання) | обгортає Loan у стані Returned або Cancelled |

## Стани видачі

| Зі стану | До стану | Умова |
|---|---|---|
| Reserved | Issued | є хоча б одна позиція |
| Reserved | Cancelled | завжди |
| Issued | Returned | завжди |
| Issued | Overdue | поточна дата пізніша за DueAt |
| Overdue | Returned | завжди |

DueAt = дата видачі + найбільший Days серед позицій; задається в момент переходу Reserved → Issued.