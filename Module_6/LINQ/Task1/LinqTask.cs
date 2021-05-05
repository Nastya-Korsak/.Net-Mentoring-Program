using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Task1.DoNotChange;

namespace Task1
{
    public static class LinqTask
    {
        public static IEnumerable<Customer> Linq1(IEnumerable<Customer> customers, decimal limit)
        {
            if (customers is null)
            {
                throw new ArgumentNullException(nameof(customers));
            }

            return customers.Where(customer => customer.Orders.Sum(order => order.Total) > limit);
        }

        public static IEnumerable<(Customer customer, IEnumerable<Supplier> suppliers)> Linq2(
            IEnumerable<Customer> customers,
            IEnumerable<Supplier> suppliers)
        {
            if (customers is null)
            {
                throw new ArgumentNullException(nameof(customers));
            }

            if (suppliers is null)
            {
                throw new ArgumentNullException(nameof(suppliers));
            }

            return customers.Select(customer =>
            (
                customer,
                suppliers.Where(supplier => supplier.Country == customer.Country && supplier.City == customer.City)
            ));
        }

        public static IEnumerable<(Customer customer, IEnumerable<Supplier> suppliers)> Linq2UsingGroup(
            IEnumerable<Customer> customers,
            IEnumerable<Supplier> suppliers)
        {
            if (customers is null)
            {
                throw new ArgumentNullException(nameof(customers));
            }

            if (suppliers is null)
            {
                throw new ArgumentNullException(nameof(suppliers));
            }

            var groupedSupplpiers = suppliers.GroupBy(supplier => new { supplier.Country, supplier.City });

            return customers
                .Select(customer =>
                (
                    customer,
                    groupedSupplpiers
                        .Where(groupedSupplpier => groupedSupplpier.Key.Country == customer.Country && groupedSupplpier.Key.City == customer.City)
                        .SelectMany(supplpier => supplpier)
                 ));
        }

        public static IEnumerable<Customer> Linq3(IEnumerable<Customer> customers, decimal limit)
        {
            if (customers is null)
            {
                throw new ArgumentNullException(nameof(customers));
            }

            return customers.Where(customer => customer.Orders.Any(order => order.Total > limit));
        }

        public static IEnumerable<(Customer customer, DateTime dateOfEntry)> Linq4(
            IEnumerable<Customer> customers)
        {
            if (customers is null)
            {
                throw new ArgumentNullException(nameof(customers));
            }

            return customers
                .Where(customer => customer.Orders.Any())
                .Select(customer => (customer, customer.Orders.Min(o => o.OrderDate)));
        }

        public static IEnumerable<(Customer customer, DateTime dateOfEntry)> Linq5(
            IEnumerable<Customer> customers)
        {
            if (customers is null)
            {
                throw new ArgumentNullException(nameof(customers));
            }

            return Linq4(customers)
                .OrderBy(customer => customer.dateOfEntry.Year)
                .ThenBy(customer => customer.dateOfEntry.Month)
                .ThenByDescending(customer => customer.customer.Orders.Sum(order => order.Total))
                .ThenBy(customer => customer.customer.CompanyName);
        }

        public static IEnumerable<Customer> Linq6(IEnumerable<Customer> customers)
        {
            if (customers is null)
            {
                throw new ArgumentNullException(nameof(customers));
            }

            return customers.Where(customer =>
                !customer.PostalCode.All(char.IsDigit)
                || string.IsNullOrEmpty(customer.Region)
                || !customer.Phone.StartsWith('('));
        }

        public static IEnumerable<Linq7CategoryGroup> Linq7(IEnumerable<Product> products)
        {
            if (products is null)
            {
                throw new ArgumentNullException(nameof(products));
            }

            return products
                .GroupBy(product => product.Category, (category, products) => new Linq7CategoryGroup()
                {
                    Category = category,
                    UnitsInStockGroup = products.GroupBy(
                        product => product.UnitsInStock,
                        product => product.UnitPrice,
                        (unitsInStock, groupedProducts) => new Linq7UnitsInStockGroup()
                        {
                            UnitsInStock = unitsInStock,
                            Prices = groupedProducts
                                        .OrderBy(price => price)
                        })
                });
        }

        public static IEnumerable<(decimal category, IEnumerable<Product> products)> Linq8(
            IEnumerable<Product> products,
            decimal cheap,
            decimal middle,
            decimal expensive)
        {
            if (products is null)
            {
                throw new ArgumentNullException(nameof(products));
            }

            return products
                .Where(product => product.UnitPrice <= expensive)
                .GroupBy(
                    product => (product.UnitPrice <= cheap) ? cheap : ((product.UnitPrice > middle) ? expensive : middle),
                    (category, products) => (category, products));
        }

        public static IEnumerable<(string city, int averageIncome, int averageIntensity)> Linq9(
            IEnumerable<Customer> customers)
        {
            if (customers is null)
            {
                throw new ArgumentNullException(nameof(customers));
            }

            return customers.GroupBy(
                customer => customer.City,
                (city, customers) =>
                    (
                        city,
                        (int)Math.Round(customers.Average(customer => customer.Orders.Sum(o => o.Total))),
                        customers.Sum(customer => customer.Orders.Count()) / customers.Count()
                ));
        }

        public static string Linq10(IEnumerable<Supplier> suppliers)
        {
            if (suppliers is null)
            {
                throw new ArgumentNullException(nameof(suppliers));
            }

            var countries = new StringBuilder();

            suppliers
                .Select(supplier => supplier.Country)
                .Distinct()
                .OrderBy(country => country.Length)
                .ThenBy(country => country)
                .ToList()
                .ForEach(country => countries.Append(country));

            return countries.ToString();
        }
    }
}
