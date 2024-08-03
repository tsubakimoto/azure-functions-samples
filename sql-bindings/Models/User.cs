/*
CREATE TABLE User (
    Name nvarchar not null primary key,
    Age int not null
)
*/

namespace AzureSqlBindingsSample.Models;

public class User
{
    public string Name { get; set; }
    public int Age { get; set; }
}
