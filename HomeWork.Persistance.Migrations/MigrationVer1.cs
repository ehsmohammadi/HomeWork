using System;
using FluentMigrator;

namespace HomeWork.Persistance.Migrations
{
    [Migration(1)]
    public class MigrationVer1 : Migration
    {
        public override void Up()
        {
            Create.Table("TestEntities")
                 .WithColumn("Id").AsInt64().PrimaryKey()
                 .WithColumn("RowVersion").AsCustom("rowversion")
                 .WithColumn("TestProperty").AsString(100).NotNullable();

            Create.Table("TestEntityItems")
                     .WithColumn("Id").AsInt64().PrimaryKey()
                     .WithColumn("RowVersion").AsCustom("rowversion")
                     .WithColumn("TestProperty").AsString(100).NotNullable()
                     .WithColumn("TestEntityId").AsInt64().Nullable().ForeignKey("TestEntities","Id");

            Create.Table("Entities")
                 .WithColumn("Id").AsInt64().PrimaryKey()
                 .WithColumn("RowVersion").AsCustom("rowversion")
                 .WithColumn("TestProperty").AsString(100).NotNullable();

            Create.Table("EntityItems")
                     .WithColumn("Id").AsInt64().PrimaryKey()
                     .WithColumn("RowVersion").AsCustom("rowversion")
                     .WithColumn("TestProperty").AsString(100).NotNullable()
                     .WithColumn("EntityId").AsInt64().Nullable().ForeignKey("Entities", "Id");

            Create.Table("EntityItems2")
                     .WithColumn("Id").AsInt64().PrimaryKey()
                     .WithColumn("RowVersion").AsCustom("rowversion")
                     .WithColumn("TestProperty").AsString(100).NotNullable()
                     .WithColumn("EntityId").AsInt64().Nullable().ForeignKey("Entities", "Id");

        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}
