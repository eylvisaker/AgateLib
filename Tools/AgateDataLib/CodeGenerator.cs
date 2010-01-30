using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using AgateLib.Data;

namespace DatabaseEditor
{
	public class CodeGenerator
	{
		public CodeGenerator()
		{
			DataStoreClass = "AgateDataImport";
			MakePublicClasses = true;
		}

		public CodeDomProvider Provider { get; set; }
		public string Directory { get; set; }
		public string Namespace { get; set; }
		public bool MakePublicClasses { get; set; }
		public string DataStoreClass { get; set; }

		public string Filename
		{
			get
			{
				return Path.Combine(Directory, "Data." + Provider.FileExtension);
			}
		}

		CodeTypeDeclaration mDataStore;
		List<CodeTypeDeclaration> mTables = new List<CodeTypeDeclaration>();
		const string rowParameterName = "row";

		public void Run(AgateDatabase dbase)
		{
			var compileUnit = new CodeCompileUnit();
			var ns = new CodeNamespace(Namespace);

			compileUnit.Namespaces.Add(ns);

			ns.Imports.Add(new CodeNamespaceImport("System"));
			ns.Imports.Add(new CodeNamespaceImport("System.Collections.Generic"));
			ns.Imports.Add(new CodeNamespaceImport("AgateLib.Data"));

			dbase.LoadAllTables();

			CreateDataStoreClass();

			ns.Types.Add(mDataStore);

			foreach (var table in dbase.Tables)
			{
				GenerateTableClass(ns, table);
			}

			CodeGeneratorOptions options = new CodeGeneratorOptions();
			options.BracingStyle = "C";
			options.BlankLinesBetweenMembers = false;

			CreateDataStoreConstructor();
			CreateDataStoreProperties();

			using (StreamWriter sourceWriter = new StreamWriter(Filename))
			{
				Provider.GenerateCodeFromCompileUnit(
					compileUnit, sourceWriter, options);
			}
		}

		private void CreateDataStoreConstructor()
		{
			CodeConstructor c = new CodeConstructor();
			c.Parameters.Add(
				new CodeParameterDeclarationExpression("AgateDatabase", "dbase"));

			c.Statements.Add(new CodeVariableDeclarationStatement("Int32", "i"));
			CodeVariableReferenceExpression iref = new CodeVariableReferenceExpression(
				"i");

			CodeArgumentReferenceExpression dbaseRef = new CodeArgumentReferenceExpression("dbase");

			foreach (var table in mTables)
			{
				var tableRef = new CodeIndexerExpression(
								new CodePropertyReferenceExpression(dbaseRef, "Tables"), 
								new CodePrimitiveExpression(table.Name));

				CodeIterationStatement forst = new CodeIterationStatement();

				forst.InitStatement = new CodeAssignStatement(iref, new CodePrimitiveExpression(0));
				forst.TestExpression = new CodeBinaryOperatorExpression(
					iref, CodeBinaryOperatorType.LessThan,
					new CodePropertyReferenceExpression(
						new CodePropertyReferenceExpression(tableRef, "Rows"), 
						"Count"));

				forst.IncrementStatement = new CodeAssignStatement(
					iref, new CodeBinaryOperatorExpression(iref, CodeBinaryOperatorType.Add, new CodePrimitiveExpression(1)));

				forst.Statements.Add(
					new CodeMethodInvokeExpression(
						new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), FieldTableName(table)),
						"Add", 
						new CodeObjectCreateExpression(
							new CodeTypeReference(table.Name), 
							new CodeIndexerExpression(
								new CodePropertyReferenceExpression(
									tableRef, "Rows"),
								iref))));



				c.Statements.Add(forst);	
			}

			c.Attributes = MemberAttributes.Public;

			mDataStore.Members.Add(c);
		}

		/// <summary>
		/// Creates properties in the data store class that contain the 
		/// data from the database.
		/// </summary>
		private void CreateDataStoreProperties()
		{
			foreach (var table in mTables)
			{
				CodeMemberField field = new CodeMemberField();

				field.Name = FieldTableName(table);
				field.Type = new CodeTypeReference(
					"List", new CodeTypeReference(new CodeTypeParameter(table.Name)));
				field.Attributes = MemberAttributes.Private;
				field.InitExpression = new CodeObjectCreateExpression(field.Type);

				CodeMemberProperty prop = new CodeMemberProperty();
				prop.Name = table.Name + "List";
				prop.Type = field.Type;
				prop.HasSet = false;
				prop.HasGet = true;

				prop.GetStatements.Add(new CodeMethodReturnStatement(
							new CodeFieldReferenceExpression(
							new CodeThisReferenceExpression(), field.Name)));

				if (MakePublicClasses)
					prop.Attributes = MemberAttributes.Public;
				else
					prop.Attributes = MemberAttributes.Assembly;

				prop.Attributes |= MemberAttributes.Final;

				mDataStore.Members.Add(field);
				mDataStore.Members.Add(prop);
			}
		}

		private static string FieldTableName(CodeTypeDeclaration table)
		{
			return "m" + table.Name + "List";
		}

		private void CreateDataStoreClass()
		{
			mDataStore = new CodeTypeDeclaration(this.DataStoreClass);
			mDataStore.IsClass = true;
			mDataStore.TypeAttributes = TypeAttributes.Sealed;

			if (MakePublicClasses)
				mDataStore.TypeAttributes |= TypeAttributes.Public;
		}

		private void GenerateTableClass(CodeNamespace ns, AgateTable table)
		{
			CodeTypeDeclaration cls = new CodeTypeDeclaration(table.Name);

			cls.IsClass = true;
			cls.TypeAttributes = TypeAttributes.Sealed;

			if (MakePublicClasses)
				cls.TypeAttributes |= TypeAttributes.Public;

			CodeConstructor constructor = new CodeConstructor();
			
			constructor.Parameters.Add(new CodeParameterDeclarationExpression(
				typeof(AgateLib.Data.AgateRow), rowParameterName));
			
			constructor.Attributes = MemberAttributes.Assembly;

			cls.Members.Add(constructor);

			foreach (var column in table.Columns)
			{
				CreateColumnProperty(cls, constructor, column);
			}

			ns.Types.Add(cls);

			mTables.Add(cls);

		}

		private void CreateColumnProperty(CodeTypeDeclaration cls, CodeConstructor constructor, AgateColumn column)
		{
			string fieldName = "m" + column.Name;

			CodeMemberField field = new CodeMemberField();
			CodeMemberProperty prop = new CodeMemberProperty();

			field.Attributes = MemberAttributes.Private;
			field.Name = fieldName;
			field.Type = new CodeTypeReference(column.FieldTypeDataType);

			prop.Attributes = MemberAttributes.Public | MemberAttributes.Final;
			prop.Name = column.Name;
			prop.Type = new CodeTypeReference(column.FieldTypeDataType);
			prop.HasGet = true;
			prop.HasSet = false;
			
			prop.GetStatements.Add(new CodeMethodReturnStatement(
				new CodeFieldReferenceExpression(
				new CodeThisReferenceExpression(), fieldName)));

			prop.Comments.Add(new CodeCommentStatement("<summary>", true));
			prop.Comments.Add(new CodeCommentStatement(column.Name + " column.", true));
			if (string.IsNullOrEmpty(column.Description) == false)
				prop.Comments.Add(new CodeCommentStatement("<br />" + column.Description, true));

			prop.Comments.Add(new CodeCommentStatement("</summary>", true));

			cls.Members.Add(field);
			cls.Members.Add(prop);

			CodeFieldReferenceExpression fieldRef = new CodeFieldReferenceExpression(
				new CodeThisReferenceExpression(), fieldName);
			CodeArgumentReferenceExpression argRef = new CodeArgumentReferenceExpression(
				rowParameterName);

			constructor.Statements.Add(
				new CodeAssignStatement(fieldRef, ConvertStringStatement(column.FieldType,
				new CodeIndexerExpression(argRef, new CodePrimitiveExpression(column.Name)))));
		}

		private CodeExpression ConvertStringStatement(FieldType type, CodeExpression codeExpression)
		{
			if (type == FieldType.String)
				return codeExpression;

			CodeMethodInvokeExpression method = new CodeMethodInvokeExpression();

			method.Parameters.Add(codeExpression);

			switch (type)
			{
				case FieldType.Int32:
				case FieldType.AutoNumber:
					method.Method = new CodeMethodReferenceExpression(
						new CodeTypeReferenceExpression("Int32"), "Parse");
					break;

				case FieldType.Int16:
					method.Method = new CodeMethodReferenceExpression(
						new CodeTypeReferenceExpression("Int16"), "Parse");
					break;

				case FieldType.Byte:
					method.Method = new CodeMethodReferenceExpression(
						new CodeTypeReferenceExpression("Byte"), "Parse");
					break;

				case FieldType.Boolean:
					method.Method = new CodeMethodReferenceExpression(
						new CodeTypeReferenceExpression("Boolean"), "Parse");
					break;

				case FieldType.DateTime:
					method.Method = new CodeMethodReferenceExpression(
						new CodeTypeReferenceExpression("DateTime"), "Parse");
					break;

				case FieldType.Decimal:
					method.Method = new CodeMethodReferenceExpression(
						new CodeTypeReferenceExpression("Decimal"), "Parse");
					break;

				case FieldType.Double:
					method.Method = new CodeMethodReferenceExpression(
						new CodeTypeReferenceExpression("Double"), "Parse");
					break;

				case FieldType.SByte:
					method.Method = new CodeMethodReferenceExpression(
						new CodeTypeReferenceExpression("SByte"), "Parse");
					break;

				case FieldType.Single:
					method.Method = new CodeMethodReferenceExpression(
						new CodeTypeReferenceExpression("Single"), "Parse");
					break;

				case FieldType.UInt16:
					method.Method = new CodeMethodReferenceExpression(
						new CodeTypeReferenceExpression("UInt16"), "Parse");
					break;

				case FieldType.UInt32:
					method.Method = new CodeMethodReferenceExpression(
						new CodeTypeReferenceExpression("UInt32"), "Parse");
					break;

				default:
					throw new NotSupportedException();
			}

			return method;

		}
	}
}
