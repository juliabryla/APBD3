using System;

namespace LegacyApp
{
    public class UserService
    {
        public bool AddUser(string firstName, string lastName, string email, DateTime dateOfBirth, int clientId)
        {
            if (IsLastAndFirstNameCorrect(firstName, lastName))
            {
                return false;
            }

            if (IsEmailOk(email))
            {
                return false;
            }

            var age = CalculatingAge(dateOfBirth);

            if (!IsUnder21(age)) return false;

            var clientRepository = new ClientRepository();
            var client = clientRepository.GetById(clientId);

            var user = new User
            {
                Client = client,
                DateOfBirth = dateOfBirth,
                EmailAddress = email,
                FirstName = firstName,
                LastName = lastName
            };

            CreditLimitCalculations(client, user);

            if (HasCreditLimitAndCreditLimitUnder500(user))
            {
                return false;
            }

            UserDataAccess.AddUser(user);
            return true;
        }

        private static bool IsUnder21(int age)
        {
            if (age < 21)
            {
                return false;
            }

            return true;
        }

        private static void CreditLimitCalculations(Client client, User user)
        {
            if (client.Type == "VeryImportantClient")
            {
                user.HasCreditLimit = false;
            }
            else if (client.Type == "ImportantClient")
            {
                using (var userCreditService = new UserCreditService())
                {
                    int creditLimit = userCreditService.GetCreditLimit(user.LastName, user.DateOfBirth);
                    creditLimit = creditLimit * 2;
                    user.CreditLimit = creditLimit;
                }
            }
            else
            {
                user.HasCreditLimit = true;
                using (var userCreditService = new UserCreditService())
                {
                    int creditLimit = userCreditService.GetCreditLimit(user.LastName, user.DateOfBirth);
                    user.CreditLimit = creditLimit;
                }
            }
        }

        private static int CalculatingAge(DateTime dateOfBirth)
        {
            var now = DateTime.Now;
            int age = now.Year - dateOfBirth.Year;
            if (now.Month < dateOfBirth.Month || (now.Month == dateOfBirth.Month && now.Day < dateOfBirth.Day)) age--;
            return age;
        }

        private static bool HasCreditLimitAndCreditLimitUnder500(User user)
        {
            return user.HasCreditLimit && user.CreditLimit < 500;
        }

        private static bool IsLastAndFirstNameCorrect(string firstName, string lastName)
        {
            return string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName);
        }

        private static bool IsEmailOk(string email)
        {
            return !email.Contains("@") && !email.Contains(".");
        }
    }
}
