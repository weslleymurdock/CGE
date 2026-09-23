using System.Reflection;
using System.Xml.Linq;

if (args.Length != 3)
{
    Console.Error.WriteLine("Usage: CardGameEngine.ApiDocGenerator <assembly> <xml-output> <package-version>");
    return 1;
}

var assemblyPath = Path.GetFullPath(args[0]);
var outputPath = Path.GetFullPath(args[1]);
var packageVersion = args[2];

Directory.CreateDirectory(Path.GetDirectoryName(outputPath)!);

var assembly = Assembly.LoadFrom(assemblyPath);
var document = File.Exists(outputPath) ? XDocument.Load(outputPath) : new XDocument(new XElement("doc"));
var root = document.Root ?? throw new InvalidOperationException("The XML documentation root element is missing.");

var assemblyElement = root.Element("assembly") ?? new XElement("assembly");
SetValue(assemblyElement, "name", assembly.GetName().Name ?? "CardGameEngine");
if (root.Element("assembly") is null)
    root.AddFirst(assemblyElement);

SetValue(root, "version", packageVersion);
SetValue(root, "generatedAtUtc", DateTimeOffset.UtcNow.ToString("O"));

var membersElement = root.Element("members") ?? new XElement("members");
if (root.Element("members") is null)
    root.Add(membersElement);

var existing = membersElement.Elements("member")
    .Where(x => x.Attribute("name") is not null)
    .ToDictionary(x => x.Attribute("name")!.Value, StringComparer.Ordinal);

const BindingFlags Flags =
    BindingFlags.Public |
    BindingFlags.NonPublic |
    BindingFlags.Instance |
    BindingFlags.Static |
    BindingFlags.DeclaredOnly;

foreach (var type in assembly.GetTypes().OrderBy(x => x.FullName, StringComparer.Ordinal))
{
    AddMember(
        "T:" + TypeDocName(type),
        "type",
        GetAccessibility(type),
        TypeKind(type),
        TypeSignature(type));

    foreach (var constructor in type.GetConstructors(Flags))
    {
        AddMember(
            "M:" + TypeDocName(type) + ".ctor#" + constructor.MetadataToken,
            "constructor",
            GetAccessibility(constructor),
            ".ctor",
            MethodSignature(constructor),
            constructor.GetParameters());
    }

    foreach (var method in type.GetMethods(Flags).Where(x => !x.IsSpecialName))
    {
        AddMember(
            "M:" + TypeDocName(type) + "." + method.Name + "#" + method.MetadataToken,
            "method",
            GetAccessibility(method),
            method.Name,
            MethodSignature(method),
            method.GetParameters(),
            method.ReturnType);
    }

    foreach (var property in type.GetProperties(Flags))
    {
        AddMember(
            "P:" + TypeDocName(type) + "." + property.Name + "#" + property.MetadataToken,
            "property",
            GetAccessibility(property),
            property.Name,
            PropertySignature(property));
    }

    foreach (var field in type.GetFields(Flags))
    {
        AddMember(
            "F:" + TypeDocName(type) + "." + field.Name + "#" + field.MetadataToken,
            "field",
            GetAccessibility(field),
            field.Name,
            FieldSignature(field));
    }

    foreach (var @event in type.GetEvents(Flags))
    {
        AddMember(
            "E:" + TypeDocName(type) + "." + @event.Name + "#" + @event.MetadataToken,
            "event",
            GetAccessibility(@event),
            @event.Name,
            EventSignature(@event));
    }
}

document.Save(outputPath);

void AddMember(
    string name,
    string kind,
    string accessibility,
    string displayName,
    string signature,
    ParameterInfo[]? parameters = null,
    Type? returnType = null)
{
    if (existing.TryGetValue(name, out var member))
    {
        SetValue(member, "accessibility", accessibility);
        SetValue(member, "kind", kind);
        SetValue(member, "signature", signature);
        AddParametersAndReturn(member, parameters, returnType);
        return;
    }

    member = new XElement(
        "member",
        new XAttribute("name", name),
        new XElement("summary", accessibility + " " + kind + " " + displayName + " declared by the CardGameEngine assembly."),
        new XElement("accessibility", accessibility),
        new XElement("kind", kind),
        new XElement("signature", signature));

    AddParametersAndReturn(member, parameters, returnType);
    membersElement.Add(member);
    existing[name] = member;
}

void AddParametersAndReturn(XElement member, ParameterInfo[]? parameters, Type? returnType)
{
    if (parameters is not null)
    {
        foreach (var parameter in parameters)
        {
            if (member.Elements("param").Any(x => x.Attribute("name")?.Value == parameter.Name))
                continue;

            member.Add(new XElement(
                "param",
                new XAttribute("name", parameter.Name ?? "parameter"),
                "Parameter of type " + FriendlyTypeName(parameter.ParameterType) + "."));
        }
    }

    if (returnType is not null &&
        returnType != typeof(void) &&
        member.Element("returns") is null)
    {
        member.Add(new XElement(
            "returns",
            "Returns " + FriendlyTypeName(returnType) + "."));
    }
}

static void SetValue(XElement parent, string name, string value)
{
    var element = parent.Element(name);
    if (element is null)
        parent.Add(new XElement(name, value));
    else
        element.Value = value;
}

static string GetAccessibility(Type type)
    => type.IsNested
        ? type.IsNestedPublic ? "public"
        : type.IsNestedFamily ? "protected"
        : type.IsNestedFamORAssem ? "protected internal"
        : type.IsNestedAssembly ? "internal"
        : type.IsNestedFamANDAssem ? "private protected"
        : "private"
        : type.IsPublic ? "public" : "internal";

static string GetAccessibility(MethodBase method)
    => method.IsPublic ? "public"
    : method.IsFamily ? "protected"
    : method.IsFamilyOrAssembly ? "protected internal"
    : method.IsFamilyAndAssembly ? "private protected"
    : method.IsAssembly ? "internal"
    : "private";

static string GetAccessibility(PropertyInfo property)
    => property.GetMethod is not null ? GetAccessibility(property.GetMethod)
    : property.SetMethod is not null ? GetAccessibility(property.SetMethod)
    : "private";

static string GetAccessibility(FieldInfo field)
    => field.IsPublic ? "public"
    : field.IsFamily ? "protected"
    : field.IsFamilyOrAssembly ? "protected internal"
    : field.IsFamilyAndAssembly ? "private protected"
    : field.IsAssembly ? "internal"
    : "private";

static string GetAccessibility(EventInfo @event)
    => @event.AddMethod is not null ? GetAccessibility(@event.AddMethod)
    : @event.RemoveMethod is not null ? GetAccessibility(@event.RemoveMethod)
    : "private";

static string TypeKind(Type type)
    => type.IsInterface ? "interface"
    : type.IsEnum ? "enum"
    : type.IsValueType ? "struct"
    : typeof(MulticastDelegate).IsAssignableFrom(type.BaseType) ? "delegate"
    : "class";

static string TypeSignature(Type type)
    => GetAccessibility(type) + " " + TypeKind(type) + " " + FriendlyTypeName(type);

static string MethodSignature(MethodBase method)
{
    var parameters = string.Join(", ", method.GetParameters().Select(x =>
        FriendlyTypeName(x.ParameterType) + " " + x.Name));
    var staticPrefix = method is MethodInfo info && info.IsStatic ? "static " : string.Empty;
    var returnType = method is MethodInfo mi ? FriendlyTypeName(mi.ReturnType) + " " : string.Empty;
    return staticPrefix + returnType + method.Name + "(" + parameters + ")";
}

static string PropertySignature(PropertyInfo property)
    => FriendlyTypeName(property.PropertyType) + " " + property.Name +
       " { " + (property.CanRead ? "get; " : string.Empty) +
       (property.CanWrite ? "set; " : string.Empty) + "}";

static string FieldSignature(FieldInfo field)
    => FriendlyTypeName(field.FieldType) + " " + field.Name;

static string EventSignature(EventInfo @event)
    => "event " + FriendlyTypeName(@event.EventHandlerType ?? typeof(Delegate)) + " " + @event.Name;

static string FriendlyTypeName(Type type)
{
    if (type.IsByRef)
        return FriendlyTypeName(type.GetElementType()!) + "&";

    if (type.IsPointer)
        return FriendlyTypeName(type.GetElementType()!) + "*";

    if (type.IsArray)
        return FriendlyTypeName(type.GetElementType()!) + "[" + new string(',', type.GetArrayRank() - 1) + "]";

    if (type.IsGenericParameter)
        return type.Name;

    if (type.IsGenericType)
    {
        var name = type.GetGenericTypeDefinition().FullName ?? type.GetGenericTypeDefinition().Name;
        var tick = name.IndexOf((char)96);
        if (tick >= 0)
            name = name[..tick];

        return name + "<" + string.Join(", ", type.GetGenericArguments().Select(FriendlyTypeName)) + ">";
    }

    return (type.FullName ?? type.Name).Replace('+', '.');
}

static string TypeDocName(Type type)
    => (type.FullName ?? type.Name).Replace('+', '.');
