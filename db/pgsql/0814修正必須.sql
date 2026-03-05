ALTER TABLE "public"."Sys_TableInfo" ADD "SearchDropNo" VARCHAR(300);
ALTER TABLE "public"."Sys_TableInfo" ADD "DbSql" TEXT;
ALTER TABLE "public"."Sys_TableInfo" ADD "ForeignField" VARCHAR(400);
ALTER TABLE "public"."Sys_TableInfo" ADD "DyPage" INTEGER;
ALTER TABLE "public"."Sys_TableInfo" ADD "DyScript" TEXT;
ALTER TABLE "public"."Sys_TableInfo" ADD "FixedSearch" INTEGER;
ALTER TABLE "public"."Sys_TableColumn" ADD "SearchColSize" DECIMAL(10,3);
ALTER TABLE "public"."Sys_TableColumn" ALTER COLUMN "ColSize" TYPE DECIMAL(10,3);
