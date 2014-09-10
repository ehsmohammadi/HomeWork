using System;
using FluentMigrator;

namespace HomeWork.Persistance.Migrations
{
    [Migration(1)]
    public class MigrationVer1 : Migration
    {
        public override void Up()
        {
            // for clear and better code this section must extract to their methods
            Create.Table("TestEntities")
                 .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                 .WithColumn("RowVersion").AsCustom("rowversion")
                 .WithColumn("TestProperty").AsString(100).NotNullable();

            Create.Table("TestEntityItems")
                     .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                     .WithColumn("RowVersion").AsCustom("rowversion")
                     .WithColumn("TestProperty").AsString(100).NotNullable()
                     .WithColumn("TestEntityId").AsInt64().Nullable().ForeignKey("TestEntities","Id");

            Create.Table("Entities")
                 .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                 .WithColumn("RowVersion").AsCustom("rowversion")
                 .WithColumn("TestProperty").AsString(100).NotNullable();

            Create.Table("EntityItems")
                     .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                     .WithColumn("RowVersion").AsCustom("rowversion")
                     .WithColumn("TestProperty").AsString(100).NotNullable()
                     .WithColumn("EntityId").AsInt64().Nullable().ForeignKey("Entities", "Id");

            Create.Table("EntityItems2")
                     .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                     .WithColumn("RowVersion").AsCustom("rowversion")
                     .WithColumn("TestProperty").AsString(100).NotNullable()
                     .WithColumn("EntityId").AsInt64().Nullable().ForeignKey("Entities", "Id");

            Create.Table("Entities3")
               .WithColumn("DbId").AsInt64().PrimaryKey().Identity()
               .WithColumn("Id").AsString(50)
               .WithColumn("RowVersion").AsCustom("rowversion")
               .WithColumn("TestProperty").AsString(100).NotNullable();

        }

        public override void Down()
        {
            Delete.Table("EntityItems2");
            Delete.Table("EntityItems");
            Delete.Table("Entities");

            Delete.Table("Entities3");

            Delete.Table("TestEntityItems");
            Delete.Table("TestEntities");
        }
    }
}
