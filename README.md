# household-manager

Household Manager is household budget management front-end application which consumes the endpoints given by the [Household Management API](https://github.com/mahidulalvi/household-management-api) for managing our 'Household Manager' application. It is built using C# and the ASP.NET framework's MVC template. This application does not handle data directly, everything is done by the `Household Management API`.

The project requirements are sourced from `Coder Foundry`. This project was originally completed on June 14, 2019.


## logic

This application lets users to manage their household budget. Household owners can invite members to their households. They can also execute CRUD functions on Categories of spending household budget, Bank Accounts and Transactions to manage and record household spending. They also have the ability to calculate the total account balance of a Bank Account.

Standard users may join/leave Households, create Transactions, edit own Transactions and display other information of their household.

All users have access to account creation and setting, resetting & changing passwords.


# project setup

For this project to run, the [Household Management API](https://github.com/mahidulalvi/household-management-api) is also required to run simultaneously. Follow the instructions in the README for setting up the API project correctly and run in localhost.

To setup the front-end application, clone this project, clean and rebuild after opening the solution in Visual Studio. See if the domain defined in the [UrlHelper](./HouseholdManager/Models/HelperClasses/BasicApiConnectionHelpers.cs) class matches with that of the running instance of the API. If not, change it to match the running instance domain of API. Then run this project in debug mode - `F5` or without debugging - `CTRL + F5`.

Good luck managing your household budget!


&nbsp;

&nbsp;

&nbsp;

# project requirements

For anyone intersted, the project specific requirements can be found [here](./CoderFoundryProjectRequirements.docx.pdf).
