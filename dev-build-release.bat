dotnet restore

dotnet build --configuration Debug
dotnet build --configuration Release

dotnet test -c Debug .\tests\TauCode.Domain.NHibernate.Tests\TauCode.Domain.NHibernate.Tests.csproj
dotnet test -c Release .\tests\TauCode.Domain.NHibernate.Tests\TauCode.Domain.NHibernate.Tests.csproj

nuget pack nuget\TauCode.Domain.NHibernate.nuspec
