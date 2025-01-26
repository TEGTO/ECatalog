using System.Diagnostics.CodeAnalysis;

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Potential Code Quality Issues", "RECS0026:Possible unassigned object created by 'new'", Justification = "Constructs add themselves to the scope in which they are created")]
[assembly: SuppressMessage("Major Bug", "S1848:Objects should not be created to be dropped immediately without being used", Justification = "<Pending>", Scope = "member", Target = "~M:Cdk.Program.Main(System.String[])")]
[assembly: SuppressMessage("Major Bug", "S1848:Objects should not be created to be dropped immediately without being used", Justification = "<Pending>", Scope = "member", Target = "~M:Cdk.EBStack.#ctor(Constructs.Construct,System.String,Amazon.CDK.IStackProps)")]
