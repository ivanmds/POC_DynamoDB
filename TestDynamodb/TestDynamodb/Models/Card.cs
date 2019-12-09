using Amazon.DynamoDBv2.DataModel;
using System;
using TestDynamodb.Repositories;

namespace TestDynamodb.Models
{
    [DynamoDBTable(RegisterTables.TABLE_NAME_CARD)]
    public class Card
    {
        public Card()
            => Created = DateTime.UtcNow;

        [DynamoDBHashKey]
        public string CardId { get; set; }
        public DateTime Created { get; set; }

        public string CompanyKey { get; set; }
        public string DocumentNumber { get; set; }
        public string ActivateCode { get; set; }


        public string BankAgency { get; set; }
        public string BanckAccount { get; set; }
        public string Account => $"{BankAgency}#{BanckAccount}";

        public string LastFourDigits { get; set; }
        public string Proxy { get; set; }
        public string CardName { get; set; }
        public int ProcessorCardId { get; set; }
        public int ProcessorAccountId { get; set; }
        public int ProcessorCustomerId { get; set; }
        public int ComboId { get; set; }
        public int ProgramId { get; set; }
        public int ChannelId { get; set; }
        public string IndexUserCard { get; set; }


        public static string BuildCardId(string companyKey, string activateCode)
            => $"{companyKey.ToUpper()}#{activateCode.ToUpper()}";

        public static string BuildIndexUserCard(string companyKey, string documentNumber, string account)
        {
            if(!string.IsNullOrEmpty(companyKey) && !string.IsNullOrEmpty(documentNumber) && !string.IsNullOrEmpty(account))
                return $"{companyKey?.ToUpper()}#{documentNumber?.ToUpper()}#{account?.ToUpper()}";

            return null;
        }


        public void LoadIds()
        {
            CardId = BuildCardId(CompanyKey, ActivateCode);
            IndexUserCard = BuildIndexUserCard(CompanyKey, DocumentNumber, Account);
        }
    }
}
