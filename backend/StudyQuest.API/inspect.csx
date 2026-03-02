var asm = System.Reflection.Assembly.LoadFrom(@"C:\Users\FatouGaye\.nuget\packages\microsoft.openapi\2.0.0\lib\net8.0\Microsoft.OpenApi.dll");

var sr = asm.GetType("Microsoft.OpenApi.OpenApiSecurityRequirement");
Console.WriteLine("=== SecurityRequirement BaseType: " + sr.BaseType?.FullName);
foreach (var p in sr.GetProperties()) Console.WriteLine("  " + p.PropertyType.Name + " " + p.Name);

Console.WriteLine("=== SecuritySchemeType enum ===");
foreach (var n in Enum.GetNames(asm.GetType("Microsoft.OpenApi.SecuritySchemeType"))) Console.WriteLine("  " + n);

var schRef = asm.GetType("Microsoft.OpenApi.OpenApiSecuritySchemeReference");
Console.WriteLine("=== OpenApiSecuritySchemeReference ===");
foreach (var c in schRef.GetConstructors()) {
    var ps = string.Join(", ", c.GetParameters().Select(p2 => p2.ParameterType.Name + " " + p2.Name));
    Console.WriteLine("  ctor(" + ps + ")");
}
foreach (var p in schRef.GetProperties()) Console.WriteLine("  " + p.PropertyType.Name + " " + p.Name + " Set:" + p.CanWrite);
