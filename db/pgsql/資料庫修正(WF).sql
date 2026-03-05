ALTER TABLE "public"."Sys_WorkFlow" ADD "TitleTemplate" VARCHAR(2000);
ALTER TABLE "public"."Sys_WorkFlowStep" ADD "StepEditForm" TEXT;
ALTER TABLE "public"."Sys_WorkFlowStep" ADD "AllowUpload" INTEGER;
ALTER TABLE "public"."Sys_WorkFlowStep" ADD "AttachType" VARCHAR(2000);
ALTER TABLE "public"."Sys_WorkFlowStep" ADD "AttachQty" INTEGER;
ALTER TABLE "public"."Sys_WorkFlowTable" ADD "TitleTemplate" VARCHAR(2000);
ALTER TABLE "public"."Sys_WorkFlowTableStep" ADD "AttachFile" VARCHAR(2000);
ALTER TABLE "public"."Sys_WorkFlowTableStep" ADD "AttachType" VARCHAR(2000);
ALTER TABLE "public"."Sys_WorkFlowTableStep" ADD "AttachQty" INTEGER;
ALTER TABLE "public"."Sys_WorkFlowTableStep" ADD "StepEditForm" TEXT;
ALTER TABLE "public"."Sys_WorkFlowTableAuditLog" ADD "AttachFile" VARCHAR(2000);
ALTER TABLE "public"."Sys_WorkFlowTableAuditLog" ADD "AttachType" VARCHAR(2000);

/*
 Navicat Premium Data Transfer

 Source Server         : 127pgsql
 Source Server Type    : PostgreSQL
 Source Server Version : 110019
 Source Host           : localhost:5432
 Source Catalog        : vol_pro_main
 Source Schema         : public

 Target Server Type    : PostgreSQL
 Target Server Version : 110019
 File Encoding         : 65001

 Date: 27/05/2025 10:28:14
*/


-- ----------------------------
-- Table structure for Sys_Notification
-- ----------------------------
DROP TABLE IF EXISTS "public"."Sys_Notification";
CREATE TABLE "public"."Sys_Notification" (
  "NotificationId" uuid NOT NULL,
  "NotificationTemplateId" uuid,
  "BusinessFunction" varchar(200) COLLATE "pg_catalog"."default",
  "TableName" varchar(200) COLLATE "pg_catalog"."default",
  "TableKey" varchar(100) COLLATE "pg_catalog"."default",
  "NotificationTitle" varchar(2000) COLLATE "pg_catalog"."default" NOT NULL,
  "NotificationContent" text COLLATE "pg_catalog"."default" NOT NULL,
  "NotificationCode" varchar(100) COLLATE "pg_catalog"."default",
  "NotificationType" varchar(100) COLLATE "pg_catalog"."default",
  "NotificationLevel" varchar(100) COLLATE "pg_catalog"."default",
  "TargetObjectType" varchar(200) COLLATE "pg_catalog"."default",
  "TargetObjectValue" varchar(4000) COLLATE "pg_catalog"."default",
  "TargetObjectText" varchar(4000) COLLATE "pg_catalog"."default",
  "IsFieldTemplate" int4,
  "PublishStatus" int4,
  "LinkUrl" varchar(255) COLLATE "pg_catalog"."default",
  "LinkType" varchar(100) COLLATE "pg_catalog"."default",
  "Remark" varchar(255) COLLATE "pg_catalog"."default",
  "Enable" int4,
  "CreateID" int4,
  "Creator" varchar(255) COLLATE "pg_catalog"."default",
  "CreateDate" timestamp(6) NOT NULL,
  "ModifyID" int4,
  "Modifier" varchar(255) COLLATE "pg_catalog"."default",
  "ModifyDate" timestamp(6)
)
;
COMMENT ON COLUMN "public"."Sys_Notification"."NotificationId" IS '主键ID';
COMMENT ON COLUMN "public"."Sys_Notification"."NotificationTemplateId" IS '模板ID';
COMMENT ON COLUMN "public"."Sys_Notification"."BusinessFunction" IS '业务功能';
COMMENT ON COLUMN "public"."Sys_Notification"."NotificationTitle" IS '通知标题';
COMMENT ON COLUMN "public"."Sys_Notification"."NotificationContent" IS '通知内容';
COMMENT ON COLUMN "public"."Sys_Notification"."NotificationCode" IS '通知编码';
COMMENT ON COLUMN "public"."Sys_Notification"."NotificationType" IS '通知类型';
COMMENT ON COLUMN "public"."Sys_Notification"."NotificationLevel" IS '通知级别';
COMMENT ON COLUMN "public"."Sys_Notification"."TargetObjectType" IS '通知对象类型';
COMMENT ON COLUMN "public"."Sys_Notification"."TargetObjectValue" IS '通知对象';
COMMENT ON COLUMN "public"."Sys_Notification"."TargetObjectText" IS '通知对象';
COMMENT ON COLUMN "public"."Sys_Notification"."IsFieldTemplate" IS '字段模板';
COMMENT ON COLUMN "public"."Sys_Notification"."PublishStatus" IS '发布状态';
COMMENT ON COLUMN "public"."Sys_Notification"."LinkUrl" IS '跳转地址';
COMMENT ON COLUMN "public"."Sys_Notification"."LinkType" IS '跳转类型';
COMMENT ON COLUMN "public"."Sys_Notification"."Remark" IS '备注';
COMMENT ON COLUMN "public"."Sys_Notification"."CreateID" IS '创建人ID';
COMMENT ON COLUMN "public"."Sys_Notification"."Creator" IS '创建人';
COMMENT ON COLUMN "public"."Sys_Notification"."CreateDate" IS '创建时间';
COMMENT ON COLUMN "public"."Sys_Notification"."ModifyID" IS '修改人Id';
COMMENT ON COLUMN "public"."Sys_Notification"."Modifier" IS '修改人';
COMMENT ON COLUMN "public"."Sys_Notification"."ModifyDate" IS '修改时间';
COMMENT ON TABLE "public"."Sys_Notification" IS '消息通知模板';

-- ----------------------------
-- Table structure for Sys_NotificationLog
-- ----------------------------
DROP TABLE IF EXISTS "public"."Sys_NotificationLog";
CREATE TABLE "public"."Sys_NotificationLog" (
  "NotificationLogId" uuid NOT NULL,
  "NotificationId" uuid,
  "NotificationTemplateId" uuid,
  "BusinessFunction" varchar(200) COLLATE "pg_catalog"."default",
  "TableName" varchar(200) COLLATE "pg_catalog"."default",
  "TableKey" varchar(100) COLLATE "pg_catalog"."default",
  "NotificationTitle" varchar(2000) COLLATE "pg_catalog"."default",
  "NotificationContent" text COLLATE "pg_catalog"."default",
  "NotificationType" varchar(100) COLLATE "pg_catalog"."default",
  "NotificationLevel" varchar(100) COLLATE "pg_catalog"."default",
  "ReceiveUserId" int4,
  "ReceiveUserName" varchar(255) COLLATE "pg_catalog"."default",
  "ReceiveUserTrueName" varchar(200) COLLATE "pg_catalog"."default",
  "LinkUrl" varchar(255) COLLATE "pg_catalog"."default",
  "LinkType" varchar(100) COLLATE "pg_catalog"."default",
  "IsRead" int4 NOT NULL,
  "ReadDate" timestamp(6),
  "Remark" varchar(255) COLLATE "pg_catalog"."default",
  "Enable" int4,
  "CreateID" int4,
  "Creator" varchar(255) COLLATE "pg_catalog"."default",
  "CreateDate" timestamp(6)
)
;
COMMENT ON COLUMN "public"."Sys_NotificationLog"."NotificationLogId" IS '主键ID';
COMMENT ON COLUMN "public"."Sys_NotificationLog"."NotificationId" IS '消息ID';
COMMENT ON COLUMN "public"."Sys_NotificationLog"."NotificationTemplateId" IS '模板ID';
COMMENT ON COLUMN "public"."Sys_NotificationLog"."BusinessFunction" IS '业务功能';
COMMENT ON COLUMN "public"."Sys_NotificationLog"."TableName" IS '业务表名';
COMMENT ON COLUMN "public"."Sys_NotificationLog"."TableKey" IS '业务表主键id';
COMMENT ON COLUMN "public"."Sys_NotificationLog"."NotificationTitle" IS '通知标题';
COMMENT ON COLUMN "public"."Sys_NotificationLog"."NotificationContent" IS '通知内容';
COMMENT ON COLUMN "public"."Sys_NotificationLog"."NotificationType" IS '通知类型';
COMMENT ON COLUMN "public"."Sys_NotificationLog"."NotificationLevel" IS '通知级别';
COMMENT ON COLUMN "public"."Sys_NotificationLog"."ReceiveUserId" IS '接收人id';
COMMENT ON COLUMN "public"."Sys_NotificationLog"."ReceiveUserName" IS '接收人帐号';
COMMENT ON COLUMN "public"."Sys_NotificationLog"."ReceiveUserTrueName" IS '接收人';
COMMENT ON COLUMN "public"."Sys_NotificationLog"."LinkUrl" IS '跳转地址';
COMMENT ON COLUMN "public"."Sys_NotificationLog"."LinkType" IS '跳转类型';
COMMENT ON COLUMN "public"."Sys_NotificationLog"."IsRead" IS '读取状态';
COMMENT ON COLUMN "public"."Sys_NotificationLog"."ReadDate" IS '读取时间';
COMMENT ON COLUMN "public"."Sys_NotificationLog"."Remark" IS '备注';
COMMENT ON COLUMN "public"."Sys_NotificationLog"."CreateID" IS '创建人ID';
COMMENT ON COLUMN "public"."Sys_NotificationLog"."Creator" IS '创建人';
COMMENT ON COLUMN "public"."Sys_NotificationLog"."CreateDate" IS '创建时间';
COMMENT ON TABLE "public"."Sys_NotificationLog" IS '消息通知记录';

-- ----------------------------
-- Table structure for Sys_NotificationTemplate
-- ----------------------------
DROP TABLE IF EXISTS "public"."Sys_NotificationTemplate";
CREATE TABLE "public"."Sys_NotificationTemplate" (
  "NotificationTemplateId" uuid NOT NULL,
  "BusinessFunction" varchar(200) COLLATE "pg_catalog"."default",
  "TableName" varchar(200) COLLATE "pg_catalog"."default",
  "TableKey" varchar(100) COLLATE "pg_catalog"."default",
  "NotificationTitle" varchar(2000) COLLATE "pg_catalog"."default" NOT NULL,
  "NotificationContent" text COLLATE "pg_catalog"."default" NOT NULL,
  "NotificationCode" varchar(100) COLLATE "pg_catalog"."default" NOT NULL,
  "NotificationType" varchar(100) COLLATE "pg_catalog"."default",
  "NotificationLevel" varchar(100) COLLATE "pg_catalog"."default",
  "TargetObjectType" varchar(200) COLLATE "pg_catalog"."default" NOT NULL,
  "TargetObjectValue" varchar(4000) COLLATE "pg_catalog"."default" NOT NULL,
  "TargetObjectText" varchar(4000) COLLATE "pg_catalog"."default",
  "IsFieldTemplate" int4,
  "PublishStatus" int4,
  "LinkUrl" varchar(255) COLLATE "pg_catalog"."default",
  "LinkType" varchar(100) COLLATE "pg_catalog"."default",
  "Remark" varchar(255) COLLATE "pg_catalog"."default",
  "Enable" int4,
  "CreateID" int4,
  "Creator" varchar(255) COLLATE "pg_catalog"."default",
  "CreateDate" timestamp(6) NOT NULL,
  "ModifyID" int4,
  "Modifier" varchar(255) COLLATE "pg_catalog"."default",
  "ModifyDate" timestamp(6)
)
;
COMMENT ON COLUMN "public"."Sys_NotificationTemplate"."NotificationTemplateId" IS '主键ID';
COMMENT ON COLUMN "public"."Sys_NotificationTemplate"."BusinessFunction" IS '业务功能';
COMMENT ON COLUMN "public"."Sys_NotificationTemplate"."NotificationTitle" IS '通知标题';
COMMENT ON COLUMN "public"."Sys_NotificationTemplate"."NotificationContent" IS '通知内容';
COMMENT ON COLUMN "public"."Sys_NotificationTemplate"."NotificationCode" IS '通知编码';
COMMENT ON COLUMN "public"."Sys_NotificationTemplate"."NotificationType" IS '通知类型';
COMMENT ON COLUMN "public"."Sys_NotificationTemplate"."NotificationLevel" IS '通知级别';
COMMENT ON COLUMN "public"."Sys_NotificationTemplate"."TargetObjectType" IS '通知对象类型';
COMMENT ON COLUMN "public"."Sys_NotificationTemplate"."TargetObjectValue" IS '通知对象';
COMMENT ON COLUMN "public"."Sys_NotificationTemplate"."TargetObjectText" IS '通知对象';
COMMENT ON COLUMN "public"."Sys_NotificationTemplate"."IsFieldTemplate" IS '字段模板';
COMMENT ON COLUMN "public"."Sys_NotificationTemplate"."PublishStatus" IS '发布状态';
COMMENT ON COLUMN "public"."Sys_NotificationTemplate"."LinkUrl" IS '跳转地址';
COMMENT ON COLUMN "public"."Sys_NotificationTemplate"."LinkType" IS '跳转类型';
COMMENT ON COLUMN "public"."Sys_NotificationTemplate"."Remark" IS '备注';
COMMENT ON COLUMN "public"."Sys_NotificationTemplate"."CreateID" IS '创建人ID';
COMMENT ON COLUMN "public"."Sys_NotificationTemplate"."Creator" IS '创建人';
COMMENT ON COLUMN "public"."Sys_NotificationTemplate"."CreateDate" IS '创建时间';
COMMENT ON COLUMN "public"."Sys_NotificationTemplate"."ModifyID" IS '修改人Id';
COMMENT ON COLUMN "public"."Sys_NotificationTemplate"."Modifier" IS '修改人';
COMMENT ON COLUMN "public"."Sys_NotificationTemplate"."ModifyDate" IS '修改时间';
COMMENT ON TABLE "public"."Sys_NotificationTemplate" IS '消息通知模板';

-- ----------------------------
-- Indexes structure for table Sys_Notification
-- ----------------------------
CREATE INDEX "NotificationCode" ON "public"."Sys_Notification" USING btree (
  "NotificationCode" COLLATE "pg_catalog"."default" "pg_catalog"."text_ops" ASC NULLS LAST
);

-- ----------------------------
-- Primary Key structure for table Sys_Notification
-- ----------------------------
ALTER TABLE "public"."Sys_Notification" ADD CONSTRAINT "Sys_Notification_pkey" PRIMARY KEY ("NotificationId");

-- ----------------------------
-- Primary Key structure for table Sys_NotificationLog
-- ----------------------------
ALTER TABLE "public"."Sys_NotificationLog" ADD CONSTRAINT "Sys_NotificationLog_pkey" PRIMARY KEY ("NotificationLogId");


