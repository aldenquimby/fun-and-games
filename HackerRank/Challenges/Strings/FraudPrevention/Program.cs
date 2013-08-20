using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace FraudPrevention
{
    internal class Record
    {
        public int OrderId { get; set; }
        public int DealId { get; set; }
        public string Email { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string CreditCard { get; set; }
    }

    public class Program
    {
        private static string GetLine()
        {
            return Console.ReadLine();
        }

        public static void Main()
        {
            var num = int.Parse(GetLine());

            var records = new List<Record>(num);
            for (var i = 0; i < num; i++)
            {
                var lineParts = GetLine().Split(',');
                records.Add(new Record
                {
                    OrderId = int.Parse(lineParts[0]),
                    DealId = int.Parse(lineParts[1]),
                    Email = lineParts[2],
                    Street = lineParts[3],
                    City = lineParts[4],
                    State = lineParts[5],
                    Zip = lineParts[6],
                    CreditCard = lineParts[7],
                });
            }

            var frauds = CheckForFraud(records.OrderBy(x => x.DealId).ToList());

            Console.WriteLine(string.Join(",", frauds));
        }

        private static IEnumerable<int> CheckForFraud(IList<Record> records)
        {
            var frauds = new HashSet<int>();

            for (var i = 0; i < records.Count - 1; i++)
            {
                var record = records[i];

                for (var j = i + 1; j < records.Count; j++)
                {
                    var other = records[j];

                    if (other.DealId != record.DealId)
                    {
                        break;
                    }

                    if (IsFraud(record, other))
                    {
                        frauds.Add(record.OrderId);
                        frauds.Add(other.OrderId);
                    }
                }
            }

            return frauds.OrderBy(x => x);
        }

        private static bool IsFraud(Record record, Record other)
        {
            if (AreEmailsEqual(record.Email, other.Email))
            {
                return true;
            }

            return AreCitiesEqual(record.City, other.City) &&
                   AreZipsEqual(record.Zip, other.Zip) &&
                   AreStatesEqual(record.State, other.State) &&
                   AreStreetsEqual(record.Street, other.Street);
        }

        private static bool AreEmailsEqual(string emailOne, string emailTwo)
        {
            emailOne = emailOne.ToUpper();
            emailTwo = emailTwo.ToUpper();

            // make sure domain is same, start at back of string until @ hit
            for (int i = emailOne.Length - 1, j = emailTwo.Length - 1; i >= 0 && emailOne[i] != '@'; i--, j--)
            {
                if (emailOne[i] != emailTwo[j])
                {
                    return false;
                }
            }

            // make sure user name is same, start at front of string until @ hit
            for (int i = 0, j = 0; i < emailOne.Length && emailOne[i] != '@'; i++, j++)
            {
                if (emailOne[i] != emailTwo[j])
                {
                    if (emailOne[i] == '.')
                    {
                        j--;
                    }
                    else if (emailTwo[j] == '.')
                    {
                        i--;
                    }
                    else if (emailOne[i] == '+' || emailTwo[j] == '+')
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (emailOne[i] == '+')
                {
                    break;
                }
            }

            return true;
        }
    
        private static bool AreCitiesEqual(string cityOne, string cityTwo)
        {
            return string.Equals(cityOne, cityTwo, StringComparison.InvariantCultureIgnoreCase);
        }

        private static bool AreZipsEqual(string zipOne, string zipTwo)
        {
            return string.Equals(zipOne, zipTwo, StringComparison.InvariantCultureIgnoreCase);
        }

        private static bool AreStreetsEqual(string s1, string s2)
        {
            s1 = s1.ToUpper().Replace("ST.", "STREET").Replace("RD.", "ROAD");
            s2 = s2.ToUpper().Replace("ST.", "STREET").Replace("RD.", "ROAD");
            return string.Equals(s1, s2);
        }

        private static readonly Dictionary<string, string> StateMap = new Dictionary<string, string>
        {
            {"IL", "ILLINOIS"},
            {"CA", "CALIFORNIA"},
            {"NY", "NEW YORK"}
        };

        private static bool AreStatesEqual(string stateOne, string stateTwo)
        {
            stateOne = stateOne.ToUpper();
            stateTwo = stateTwo.ToUpper();

            if (StateMap.ContainsKey(stateOne))
            {
                stateOne = StateMap[stateOne];
            }

            if (StateMap.ContainsKey(stateTwo))
            {
                stateTwo = StateMap[stateTwo];
            }

            return string.Equals(stateOne, stateTwo);
        }
    }
}