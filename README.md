# UNIBUDGET – Student Budget Management System 🎓💰

🔗 **GitHub Repository:** [YourUsername/UNIBUDGET](https://github.com/YourUsername/UNIBUDGET)

![C#](https://img.shields.io/badge/c%23-%23239120.svg?style=for-the-badge&logo=c-sharp&logoColor=white)
![Windows Forms](https://img.shields.io/badge/Windows%20Forms-%235C2D91.svg?style=for-the-badge&logo=.net&logoColor=white)
![ClosedXML](https://img.shields.io/badge/ClosedXML-Excel_Interop-success?style=for-the-badge)

A lightweight, desktop-based budget tracking application designed specifically for university students. The application provides a simple, intuitive interface to manage personal finances, track daily expenses, and achieve savings goals without requiring a complex database setup. 

Data persistence is handled entirely via a local Excel file (`UNIBUDGET.xlsx`) utilizing the **ClosedXML** library, making the app highly portable and easy to deploy.

## 🚀 Key Features

* **User Authentication:** Secure login and registration system. Each user's financial data is isolated within the shared Excel database.
* **Dashboard & Balance Tracking:** Real-time calculation of current balance, total income, and total expenses.
* **Income & Expense Management:** Easy entry of financial operations with categorization (Food, Health, Fuel, Entertainment, Taxes, Other).
* **Savings Goals:** Create up to 3 active savings goals. Visual pie charts track the progress between deposited funds and the target amount.
* **Subaccounts:** Generate and manage up to 3 randomized subaccounts for separating funds. Includes a dedicated module for transferring money from the main balance.
* **Data Visualization:** Built-in charts to analyze cash flow, generating specific reports for Income, Expenses, or combined summaries.

## 🛠️ Technology Stack

* **Language:** C# (.NET Framework)
* **UI/UX:** Windows Forms (WinForms)
* **Data Storage:** Microsoft Excel (`.xlsx`)
* **Libraries:** ClosedXML (for reading/writing Excel spreadsheets without requiring MS Office Interop)

## ⚙️ Setup & Installation

1. Clone the repository.
2. Open the solution `.sln` file in Visual Studio.
3. Ensure the **ClosedXML** NuGet package is restored.
4. Verify that the `UNIBUDGET.xlsx` file is located in the parent directory of the executable (as defined by `Directory.GetParent(Environment.CurrentDirectory).Parent.FullName`).
5. Build and run the application.

## 📊 Database Structure (Excel Sheets)
The `UNIBUDGET.xlsx` file acts as a relational-style database containing the following sheets:
* `użytkownicy` (Users: Login, Password, Email)
* `wpłaty` (Income: User, Date, Title, Amount)
* `wydatki` (Expenses: User, Date, Category, Title, Amount)
* `oszczędności` (Savings Goals: User, Title, Target Amount, Current Amount, Status)
* `subkonta` (Subaccounts: User, Account Number, Balance)