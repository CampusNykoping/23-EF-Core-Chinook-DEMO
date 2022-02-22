using ChinookDemo.Models;

SelectMany();
//SelectOne("Bohemian Rhapsody");
//SelectOne("Love Kills");
//SelectOneFind(1);
//SelectOneFind(1111);
//ProjectToConcreteType();
//ProjectToAnonymousType();
//joinTwoTablesExample1();
//joinTwoTablesExample2();
//joinTwoTablesExample3();
//joinTwoTablesExample4();


Console.Write("Tryck på en tangent för att fortsätta");
Console.ReadKey();

void SelectMany()
{
    using (var db = new ChinookContext())
    {
        var customers = db.Customers
                        .Where(c => c.FirstName.StartsWith("A"))
                        .OrderBy(c => c.LastName)
                        .ToList();

        var customers2 = (from c in db.Customers
                          where c.FirstName.StartsWith("A")
                          orderby c.LastName
                          select c)
                         .ToList();

        foreach (var c in customers2)
        {
            Console.WriteLine($"{c.CustomerId}: {c.FirstName} {c.LastName} Email: {c.Email}, Address: {c.Address}");
        }
    }
}

void SelectOne(string trackName)
{
    using (var db = new ChinookContext())
    {
        try
        {
            //var track = db.Tracks.Where(f => f.Name == trackName).Single();
            var track = db.Tracks.Where(f => f.Name == trackName).SingleOrDefault();
            if (track != null)
            {
                Console.WriteLine("{0} {1} ", track.Name, track.Composer);
            }
            else
            {
                Console.WriteLine("Track not found");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Track not found but exception thrown " + ex.Message);
        }
    }
}

void SelectOneFind(int id)
{
    using (ChinookContext db = new ChinookContext())
    {
        var employee = db.Employees.Find(id);

        if (employee != null)
        {
            Console.WriteLine("{0} {1} Email @ {2}", employee.FirstName, employee.LastName, employee.Email);
        }
        else
        {
            Console.WriteLine($"No record found for id = {id}");
        }
    }
}

void ProjectToConcreteType()
{
    using (ChinookContext db = new ChinookContext())
    {
        List<CustomerModel> Customers = db.Customers
                            .Select(p =>
                                    new CustomerModel
                                    {
                                        FirstName = p.FirstName,
                                        LastName = p.LastName,
                                        Email = p.Email
                                    })
                            .ToList();


        foreach (var p in Customers)
        {
            Console.WriteLine("{0} {1} Email ID : {2}", p.FirstName, p.LastName, p.Email);
        }
    }
}

void ProjectToAnonymousType()
{
    //Method Syntax
    using (ChinookContext db = new ChinookContext())
    {
        var Customers = db.Customers.
                        Select(p => new
                        {
                            FirstName = p.FirstName,
                            LastName = p.LastName,
                            Email = p.Email
                        })
                        .ToList();
        foreach (var p in Customers)
        {
            Console.WriteLine("{0} {1} Email ID : {2}", p.FirstName, p.LastName, p.Email);
        }
    }
}

static void joinTwoTablesExample1()
{
    //Query Syntax
    using (ChinookContext db = new ChinookContext())
    {
        var Track = (from o in db.Tracks
                     join i in db.MediaTypes
                     on o.MediaTypeId equals i.MediaTypeId
                     select new
                     {
                         Name = o.Name,
                         Composer = o.Composer,
                         MediaType = i.Name
                     }).Take(5);

        foreach (var t in Track)
        {
            Console.WriteLine("{0} {1} {2}", t.Name, t.Composer, t.MediaType);
        }
    }
    Console.WriteLine("Press any key to continue");
    Console.ReadKey();
}

static void joinTwoTablesExample2()
{
    //Method Syntax
    using (ChinookContext db = new ChinookContext())
    {

        var track = db.Tracks
            .Join(db.MediaTypes,
                o => o.MediaTypeId,
                i => i.MediaTypeId,
                (o, i) =>
                new
                {
                    Name = o.Name,
                    Composer = o.Composer,
                    MediaType = i.Name
                }
            ).Take(5);

        foreach (var t in track)
        {
            Console.WriteLine("{0} {1} {2}", t.Name, t.Composer, t.MediaType);
        }
    }
}

static void joinTwoTablesExample3()
{
    //Method Syntax
    using (ChinookContext db = new ChinookContext())
    {

        var track = db.Customers
            .Join(db.Employees,
                f => new { f1 = f.SupportRepId.Value, f2 = f.State },
                s => new { f1 = s.EmployeeId, f2 = s.State },
                (f, s) =>
                new
                {
                    CustomerName = f.FirstName,
                    CustomerState = f.State,
                    EmployeeName = s.FirstName,
                    EmployeeState = s.State,
                }
            ).Take(5);


        foreach (var t in track)
        {
            Console.WriteLine("{0} {1} {2} {3}", t.CustomerName, t.CustomerState, t.EmployeeName, t.EmployeeState);
        }
    }
}

static void joinTwoTablesExample4()
{
    //Method Syntax
    using (ChinookContext db = new ChinookContext())
    {
        var track = (from f in db.Customers
                     join s in db.Employees
                     on new { f1 = f.SupportRepId.Value, f2 = f.State } equals new { f1 = s.EmployeeId, f2 = s.State }
                     select new
                     {
                         CustomerName = f.FirstName,
                         CustomerState = f.State,
                         EmployeeName = s.FirstName,
                         EmployeeState = s.State,
                     }).Take(5);

        foreach (var t in track)
        {
            Console.WriteLine("{0} {1} {2} {3}", t.CustomerName, t.CustomerState, t.EmployeeName, t.EmployeeState);
        }
    }
}
