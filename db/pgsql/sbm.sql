/*
 Navicat Premium Data Transfer

 Source Server         : 120pgsql
 Source Server Type    : PostgreSQL
 Source Server Version : 110019
 Source Host           : 120.48.115.252:5432
 Source Catalog        : vol_pro_test
 Source Schema         : public

 Target Server Type    : PostgreSQL
 Target Server Version : 110019
 File Encoding         : 65001

 Date: 07/05/2023 23:25:10
*/


-----------------------------
-- Sequence structure for sbm_partner_id_seq
------------------------------
DROP SEQUENCE IF EXISTS "public"."sbm_partner_id_seq" cascade;
CREATE SEQUENCE "public"."sbm_partner_id_seq" 
INCREMENT 1
MINVALUE  1000
MAXVALUE 9223372036854775807
START 1000
CACHE 1;


------------------------------
-- Table structure for sbm_partner
------------------------------
DROP TABLE IF EXISTS "public"."sbm_partner" cascade;
CREATE TABLE "public"."sbm_partner" (
  "id" int4 NOT NULL DEFAULT nextval('sbm_partner_id_seq'::regclass),
  "name" varchar(200) COLLATE "pg_catalog"."default" NOT NULL,
  "partner_type" int4,
  "parent_id" int4,
  "address"varchar(300),
  "tel" varchar(30),
  "mobile" varchar(30),
  "email" varchar(200),
  "memo" text,
  "CreateID" int4,
  "Creator" varchar(30) COLLATE "pg_catalog"."default",
  "CreateDate" timestamp(6),
  "ModifyID" int4,
  "Modifier" varchar(30) COLLATE "pg_catalog"."default",
  "ModifyDate" timestamp(6)
)
;


alter table sbm_partner add column vat varchar(8) ;
alter table sbm_partner add column if not exists bus_type int4 ;

alter table sbm_partner add column if not exists position varchar(100) ;

alter table sbm_partner add column if not exists bank_code varchar(20) ;
alter table sbm_partner add column if not exists bank_name varchar(100) ;
alter table sbm_partner add column if not exists bank_bcode varchar(20) ;
alter table sbm_partner add column if not exists bank_bname varchar(100) ;
alter table sbm_partner add column if not exists account_name varchar(150) ;
alter table sbm_partner add column if not exists account_no varchar(50) ;
alter table sbm_partner add column if not exists account_window varchar(200) ;

COMMENT ON TABLE  "public"."sbm_partner" is '合作夥伴主檔' ;
COMMENT ON COLUMN "public"."sbm_partner"."id" IS 'ID';
COMMENT ON COLUMN "public"."sbm_partner"."name" IS '夥伴名稱';
COMMENT ON COLUMN "public"."sbm_partner"."partner_type" IS '公司/個人';
COMMENT ON COLUMN "public"."sbm_partner"."parent_id" IS '所屬公司';
COMMENT ON COLUMN "public"."sbm_partner"."address" IS '公司地址';
COMMENT ON COLUMN "public"."sbm_partner"."tel" IS '電話';
COMMENT ON COLUMN "public"."sbm_partner"."mobile" IS '行動電話';
COMMENT ON COLUMN "public"."sbm_partner"."email" IS 'EMail';
COMMENT ON COLUMN "public"."sbm_partner"."memo" IS '備註';
COMMENT ON COLUMN "public"."sbm_partner"."vat" IS '統編';
COMMENT ON COLUMN "public"."sbm_partner"."position" IS '職位';
COMMENT ON COLUMN "public"."sbm_partner"."bus_type" IS '客戶/供應商';
COMMENT ON COLUMN "public"."sbm_partner"."bank_code" IS '銀行代號';
COMMENT ON COLUMN "public"."sbm_partner"."bank_name" IS '銀行名稱';
COMMENT ON COLUMN "public"."sbm_partner"."bank_bcode" IS '分行代號';
COMMENT ON COLUMN "public"."sbm_partner"."bank_bname" IS '分行名稱';
COMMENT ON COLUMN "public"."sbm_partner"."account_name" IS '帳戶名稱';
COMMENT ON COLUMN "public"."sbm_partner"."account_no" IS '帳號';
COMMENT ON COLUMN "public"."sbm_partner"."account_window" IS '帳務聯絡窗口';
4



-----------------------------
-- Sequence structure for sbm_saleorder_id_seq
------------------------------
DROP SEQUENCE IF EXISTS "public"."sbm_saleorder_id_seq" cascade;
CREATE SEQUENCE "public"."sbm_saleorder_id_seq" 
INCREMENT 1
MINVALUE  1000
MAXVALUE 9223372036854775807
START 1000
CACHE 1;


-- ----------------------------
-- Table structure for sbm_sale_order
-- ----------------------------
DROP TABLE IF EXISTS "public"."sbm_sale_order" cascade;
CREATE TABLE "public"."sbm_sale_order" (
  "sale_id" int4 NOT NULL  DEFAULT nextval('sbm_saleorder_id_seq'::regclass),
  "name" varchar(30),
  "sale_date" date,
  "sales" int4,
  "partner_company" int4,
  "partner_contact" int4,
  "untax_amount" float,
  "discount_amount" float,
  "tax" float,
  "tot_amount" float,
  "description" text,
  "CreateDate" timestamp(6),
  "CreateID" int4,
  "Creator" varchar(30) COLLATE "pg_catalog"."default",
  "Modifier" varchar(30) COLLATE "pg_catalog"."default",
  "ModifyDate" timestamp(6),
  "ModifyID" int4
)
;

SELECT setval('"public"."sbm_saleorder_id_seq"', 1001, true);

alter table sbm_sale_order add column if not exists sales_mobile varchar(11) ;
alter table sbm_sale_order add column if not exists partner_mobile varchar(20);
alter table sbm_sale_order add column if not exists partner_email varchar(100);
alter table sbm_sale_order add column if not exists sales_email varchar(100) ;
alter table sbm_sale_order add column if not exists company_id int4 ;
alter table sbm_sale_order add column if not exists bu_logo varchar(200) ;
alter table sbm_sale_order add column if not exists bu_invoice_stamp varchar(200) ;
alter table sbm_sale_order add column if not exists bu_bank_info varchar(300) ;


COMMENT ON TABLE  "public"."sbm_sale_order" is '報價單主檔' ;
COMMENT ON COLUMN "public"."sbm_sale_order"."sale_id" IS 'ID';
COMMENT ON COLUMN "public"."sbm_sale_order"."name" IS '報價單號';
COMMENT ON COLUMN "public"."sbm_sale_order"."sale_date" IS '報價日期';
COMMENT ON COLUMN "public"."sbm_sale_order"."sales" IS '業務員';
COMMENT ON COLUMN "public"."sbm_sale_order"."name" IS '報價單號';
COMMENT ON COLUMN "public"."sbm_sale_order"."partner_company" IS '客戶名稱';
COMMENT ON COLUMN "public"."sbm_sale_order"."partner_contact" IS '客戶聯絡人';
COMMENT ON COLUMN "public"."sbm_sale_order"."untax_amount" IS '未稅小計';
COMMENT ON COLUMN "public"."sbm_sale_order"."discount_amount" IS '優惠小計';
COMMENT ON COLUMN "public"."sbm_sale_order"."tax" IS '稅金';
COMMENT ON COLUMN "public"."sbm_sale_order"."tot_amount" IS '含稅合計';
COMMENT ON COLUMN "public"."sbm_sale_order"."description" IS '備註';
COMMENT ON COLUMN "public"."sbm_sale_order"."bu_logo" IS 'LOGO';
COMMENT ON COLUMN "public"."sbm_sale_order"."bu_invoice_stamp" IS '發票章';
COMMENT ON COLUMN "public"."sbm_sale_order"."bu_bank_info" IS '匯款資訊';




-----------------------------
-- Sequence structure for sbm_saleorderline_id_seq
------------------------------
DROP SEQUENCE IF EXISTS "public"."sbm_saleorderline_id_seq" cascade;
CREATE SEQUENCE "public"."sbm_saleorderline_id_seq" 
INCREMENT 1
MINVALUE  1000
MAXVALUE 9223372036854775807
START 1000
CACHE 1;


-- ----------------------------
-- Table structure for sbm_sale_order_line
-- ----------------------------
DROP TABLE IF EXISTS "public"."sbm_sale_order_line" cascade;
CREATE TABLE "public"."sbm_sale_order_line" (
  "id" int4 NOT NULL DEFAULT nextval('sbm_saleorderline_id_seq'::regclass),
  "sale_id" int4,
  "parent_id" int4,
  "sale_item" varchar(20),
  "prod_name" TEXT,
  "prod_num" float,
  "prod_uom" varchar(10),
  "price_unit" float,
  "price_sub" float,
  "prod_desc" varchar(300),
  "CreateDate" timestamp(6),
  "CreateID" int4,
  "Creator" varchar(30) COLLATE "pg_catalog"."default",
  "Modifier" varchar(30) COLLATE "pg_catalog"."default",
  "ModifyDate" timestamp(6),
  "ModifyID" int4
)
;

SELECT setval('"public"."sbm_saleorderline_id_seq"', 1001, true);

COMMENT ON TABLE  "public"."sbm_sale_order_line" is '報價單明細檔' ;
COMMENT ON COLUMN "public"."sbm_sale_order_line"."id" IS 'ID';
COMMENT ON COLUMN "public"."sbm_sale_order_line"."sale_id" IS '主檔關聯';
COMMENT ON COLUMN "public"."sbm_sale_order_line"."parent_id" IS '上階ID';
COMMENT ON COLUMN "public"."sbm_sale_order_line"."sale_item" IS '項次';
COMMENT ON COLUMN "public"."sbm_sale_order_line"."prod_name" IS '產品';
COMMENT ON COLUMN "public"."sbm_sale_order_line"."prod_num" IS '數量';
COMMENT ON COLUMN "public"."sbm_sale_order_line"."prod_uom" IS '單位';
COMMENT ON COLUMN "public"."sbm_sale_order_line"."price_unit" IS '單價';
COMMENT ON COLUMN "public"."sbm_sale_order_line"."price_sub" IS '小計';
COMMENT ON COLUMN "public"."sbm_sale_order_line"."prod_desc" IS '備註';


-----------------------------
-- Sequence structure for sbm_saleorderno_id_seq
------------------------------
DROP SEQUENCE IF EXISTS "public"."sbm_saleorderno_id_seq" cascade;
CREATE SEQUENCE "public"."sbm_saleorderno_id_seq" 
INCREMENT 1
MINVALUE  1000
MAXVALUE 9223372036854775807
START 1000
CACHE 1;

-- ----------------------------
-- Table structure for sbm_saleorderno
-- ----------------------------
DROP TABLE IF EXISTS "public"."sbm_saleorderno" cascade;
CREATE TABLE "public"."sbm_saleorderno" (
  "id" int4 NOT NULL DEFAULT nextval('sbm_saleorderno_id_seq'::regclass),
  "bu_id" int4,
  "prefix_code" varchar(10),
  "sale_year" varchar,
  "sale_month" varchar,
  "seq" int4
  )
;


-----------------------------
-- Sequence structure for sbm_reqpurchase_id_seq
------------------------------
DROP SEQUENCE IF EXISTS "public"."sbm_reqpurchase_id_seq" cascade;
CREATE SEQUENCE "public"."sbm_reqpurchase_id_seq" 
INCREMENT 1
MINVALUE  1000
MAXVALUE 9223372036854775807
START 1000
CACHE 1;




-- ----------------------------
-- Table structure for sbm_require_purchase
-- ----------------------------
DROP TABLE IF EXISTS "public"."sbm_require_purchase" cascade;
CREATE TABLE "public"."sbm_require_purchase" (
  "req_id" int4 NOT NULL DEFAULT nextval('sbm_reqpurchase_id_seq'::regclass),
  "sale_id" int4,
  "name" varchar(30),
  "partner_company" int4,
  "partner_contact" int4,
  "untax_amount" float,
  "discount_amount" float,
  "tax" float,
  "tot_amount" float,
  "description" text,
  "CreateDate" timestamp(6),
  "CreateID" int4,
  "Creator" varchar(30) COLLATE "pg_catalog"."default",
  "Modifier" varchar(30) COLLATE "pg_catalog"."default",
  "ModifyDate" timestamp(6),
  "ModifyID" int4

)
;

alter table sbm_require_purchase add column if not exists end_user int4 ;

COMMENT ON TABLE  "public"."sbm_require_purchase" is '詢價單主檔' ;
COMMENT ON COLUMN "public"."sbm_require_purchase"."sale_id" IS 'SO ID';
COMMENT ON COLUMN "public"."sbm_require_purchase"."name" IS '詢價單號';
COMMENT ON COLUMN "public"."sbm_require_purchase"."partner_company" IS '詢價廠商';
COMMENT ON COLUMN "public"."sbm_require_purchase"."partner_contact" IS '廠商聯絡人';
COMMENT ON COLUMN "public"."sbm_require_purchase"."name" IS '報價單號';
COMMENT ON COLUMN "public"."sbm_require_purchase"."partner_company" IS '客戶名稱';
COMMENT ON COLUMN "public"."sbm_require_purchase"."partner_contact" IS '客戶聯絡人';
COMMENT ON COLUMN "public"."sbm_require_purchase"."untax_amount" IS '未稅小計';
COMMENT ON COLUMN "public"."sbm_require_purchase"."discount_amount" IS '優惠小計';
COMMENT ON COLUMN "public"."sbm_require_purchase"."tax" IS '稅金';
COMMENT ON COLUMN "public"."sbm_require_purchase"."tot_amount" IS '含稅合計';
COMMENT ON COLUMN "public"."sbm_require_purchase"."description" IS '備註';
COMMENT ON COLUMN "public"."sbm_require_purchase"."end_user" IS '終端客戶';


-----------------------------
-- Sequence structure for sbm_reqpurchaseline_id_seq
------------------------------
DROP SEQUENCE IF EXISTS "public"."sbm_reqpurchaseline_id_seq" cascade;
CREATE SEQUENCE "public"."sbm_reqpurchaseline_id_seq" 
INCREMENT 1
MINVALUE  1000
MAXVALUE 9223372036854775807
START 1000
CACHE 1;


-- ----------------------------
-- Table structure for sbm_require_purchase_line
-- ----------------------------
DROP TABLE IF EXISTS "public"."sbm_require_purchase_line" cascade;
CREATE TABLE "public"."sbm_require_purchase_line" (
  "id" int4 NOT NULL DEFAULT nextval('sbm_reqpurchaseline_id_seq'::regclass),
  "req_id" int4,
  "req_item" varchar(20),
  "prod_name" TEXT,
  "prod_num" float,
  "prod_uom" varchar(10),
  "price_unit" float,
  "price_sub" float,
  "prod_desc" varchar(300),
  "CreateDate" timestamp(6),
  "CreateID" int4,
  "Creator" varchar(30) COLLATE "pg_catalog"."default",
  "Modifier" varchar(30) COLLATE "pg_catalog"."default",
  "ModifyDate" timestamp(6),
  "ModifyID" int4
)
;

SELECT setval('"public"."sbm_reqpurchaseline_id_seq"', 1001, true);

COMMENT ON TABLE  "public"."sbm_require_purchase_line" is '報價單明細檔' ;
COMMENT ON COLUMN "public"."sbm_require_purchase_line"."id" IS 'ID';
COMMENT ON COLUMN "public"."sbm_require_purchase_line"."req_id" IS '主檔關聯';
COMMENT ON COLUMN "public"."sbm_require_purchase_line"."req_item" IS '項次';
COMMENT ON COLUMN "public"."sbm_require_purchase_line"."prod_name" IS '產品';
COMMENT ON COLUMN "public"."sbm_require_purchase_line"."prod_num" IS '數量';
COMMENT ON COLUMN "public"."sbm_require_purchase_line"."prod_uom" IS '單位';
COMMENT ON COLUMN "public"."sbm_require_purchase_line"."price_unit" IS '單價';
COMMENT ON COLUMN "public"."sbm_require_purchase_line"."price_sub" IS '小計';
COMMENT ON COLUMN "public"."sbm_require_purchase_line"."prod_desc" IS '備註';


-----------------------------
-- Sequence structure for sbm_reqpurchasedoc_id_seq
------------------------------
DROP SEQUENCE IF EXISTS "public"."sbm_reqpurchasedoc_id_seq" cascade;
CREATE SEQUENCE "public"."sbm_reqpurchasedoc_id_seq" 
INCREMENT 1
MINVALUE  1000
MAXVALUE 9223372036854775807
START 1000
CACHE 1;

-- ----------------------------
-- Table structure for sbm_require_purchase_doc
-- ----------------------------
DROP TABLE IF EXISTS "public"."sbm_require_purchase_doc" cascade;
CREATE TABLE "public"."sbm_require_purchase_doc" (
  "id" int4 NOT NULL DEFAULT nextval('sbm_reqpurchasedoc_id_seq'::regclass),
  "req_id" int4,
  "partner_company" int4,
  "partner_contact" int4,
  "name" varchar(300),
  "req_doc" text, 
  "CreateDate" timestamp(6),
  "CreateID" int4,
  "Creator" varchar(30) COLLATE "pg_catalog"."default",
  "Modifier" varchar(30) COLLATE "pg_catalog"."default",
  "ModifyDate" timestamp(6),
  "ModifyID" int4
)
;

COMMENT ON TABLE  "public"."sbm_require_purchase_doc" is '廠商詢價資料檔' ;
COMMENT ON COLUMN "public"."sbm_require_purchase_doc"."id" IS 'ID';
COMMENT ON COLUMN "public"."sbm_require_purchase_doc"."req_id" IS '主檔關聯';
COMMENT ON COLUMN "public"."sbm_require_purchase_doc"."partner_company" IS '廠商名稱';
COMMENT ON COLUMN "public"."sbm_require_purchase_doc"."partner_contact" IS '廠商聯絡人';
COMMENT ON COLUMN "public"."sbm_require_purchase_doc"."name" IS '詢價案名';
COMMENT ON COLUMN "public"."sbm_require_purchase_doc"."req_doc" IS '詢價文件檔';



-----------------------------
-- Sequence structure for sbm_bu_id_seq
------------------------------
DROP SEQUENCE IF EXISTS "public"."sbm_bu_id_seq" cascade;
CREATE SEQUENCE "public"."sbm_bu_id_seq" 
INCREMENT 1
MINVALUE  1
MAXVALUE 9223372036854775807
START 1
CACHE 1;


-- ----------------------------
-- Table structure for sbm_bu
-- ----------------------------
DROP TABLE IF EXISTS "public"."sbm_bu" cascade;
CREATE TABLE "public"."sbm_bu" (
  "id" int4 NOT NULL DEFAULT nextval('sbm_bu_id_seq'::regclass),
  "bu_name" varchar(50),
  "bu_logo" varchar(200),
  "bu_invoice_stamp" varchar(200)
)
;


alter table sbm_bu add column if not exists bu_bankno varchar(10) ;
alter table sbm_bu add column if not exists bu_bankname varchar(100) ;
alter table sbm_bu add column if not exists bu_bankbno varchar(10) ;
alter table sbm_bu add column if not exists bu_bankbname varchar(10) ;
alter table sbm_bu add column if not exists bu_bankaccno varchar(30) ;
alter table sbm_bu add column if not exists bu_bankaccname varchar(100) ;


COMMENT ON TABLE  "public"."sbm_bu" is '事業單位' ;
COMMENT ON COLUMN "public"."sbm_bu"."id" IS 'ID';
COMMENT ON COLUMN "public"."sbm_bu"."bu_name" IS '單位名稱';
COMMENT ON COLUMN "public"."sbm_bu"."bu_logo" IS '單位LOGO';
COMMENT ON COLUMN "public"."sbm_bu"."bu_invoice_stamp" IS '單位發票章';
COMMENT ON COLUMN "public"."sbm_bu"."bu_bankno" IS '銀行代碼';
COMMENT ON COLUMN "public"."sbm_bu"."bu_bankname" IS '銀行名稱';
COMMENT ON COLUMN "public"."sbm_bu"."bu_bankbno" IS '分行代碼';
COMMENT ON COLUMN "public"."sbm_bu"."bu_bankbname" IS '分行名稱';
COMMENT ON COLUMN "public"."sbm_bu"."bu_bankaccno" IS '帳號';
COMMENT ON COLUMN "public"."sbm_bu"."bu_bankaccname" IS '戶名';





/* insert into sbm_bu(bu_name) values ('連瑟') ;
insert into sbm_bu(bu_name) values ('歐度') ;
insert into sbm_bu(bu_name) values ('創維') ; */


-----------------------------
-- Sequence structure for sbm_reqopen_id_seq
------------------------------
DROP SEQUENCE IF EXISTS "public"."sbm_reqopen_id_seq" cascade;
CREATE SEQUENCE "public"."sbm_reqopen_id_seq" 
INCREMENT 1
MINVALUE  1001
MAXVALUE 9223372036854775807
START 1001
CACHE 1;


-- ----------------------------
-- Table structure for sbm_reqopen
-- ----------------------------
DROP TABLE IF EXISTS "public"."sbm_reqopen" cascade;
CREATE TABLE "public"."sbm_reqopen" (
  "id" int4 NOT NULL DEFAULT nextval('sbm_reqopen_id_seq'::regclass),
  "so_id" int4,
  "partner_name" int4,
  "partner_contact" int4,
  "CreateDate" timestamp(6),
  "CreateID" int4,
  "Creator" varchar(30) COLLATE "pg_catalog"."default",
  "Modifier" varchar(30) COLLATE "pg_catalog"."default",
  "ModifyDate" timestamp(6),
  "ModifyID" int4
)
;


COMMENT ON TABLE  "public"."sbm_reqopen" is '詢價單建立' ;
COMMENT ON COLUMN "public"."sbm_reqopen"."id" IS 'ID';
COMMENT ON COLUMN "public"."sbm_reqopen"."so_id" IS '報價單號';
COMMENT ON COLUMN "public"."sbm_reqopen"."partner_name" IS '報價廠商';
COMMENT ON COLUMN "public"."sbm_reqopen"."partner_contact" IS '廠商代表';

-----------------------------
-- Sequence structure for sbm_reqno_id_seq
------------------------------
DROP SEQUENCE IF EXISTS "public"."sbm_reqno_id_seq" cascade;
CREATE SEQUENCE "public"."sbm_reqno_id_seq" 
INCREMENT 1
MINVALUE  1000
MAXVALUE 9223372036854775807
START 1000
CACHE 1;

-- ----------------------------
-- Table structure for sbm_reqno
-- ----------------------------
DROP TABLE IF EXISTS "public"."sbm_reqno" cascade;
CREATE TABLE "public"."sbm_reqno" (
  "id" int4 NOT NULL DEFAULT nextval('sbm_reqno_id_seq'::regclass),
  "bu_id" int4,
  "prefix_code" varchar(10),
  "req_year" varchar,
  "req_month" varchar,
  "seq" int4
  )
;



-----------------------------
-- Sequence structure for sbm_stockpicking_id_seq
------------------------------
DROP SEQUENCE IF EXISTS "public"."sbm_stockpicking_id_seq" cascade;
CREATE SEQUENCE "public"."sbm_stockpicking_id_seq" 
INCREMENT 1
MINVALUE  1000
MAXVALUE 9223372036854775807
START 1000
CACHE 1;


-- ----------------------------
-- Table structure for sbm_stock_picking
-- ----------------------------
DROP TABLE IF EXISTS "public"."sbm_stock_picking" cascade;
CREATE TABLE "public"."sbm_stock_picking" (
  "picking_id" int4 NOT NULL  DEFAULT nextval('sbm_stockpicking_id_seq'::regclass),
  "name" varchar(30),
  "picking_date" date,
  "picking_type" int4,
  "origin" varchar,
  "location_source" varchar(200),
  "location_destination" varchar(200),
  "picking_owner" int4,
  "company_id" int4,
  "partner_company" int4,
  "partner_contact" int4,
  "bu_logo" varchar(200),
  "CreateDate" timestamp(6),
  "CreateID" int4,
  "Creator" varchar(30) COLLATE "pg_catalog"."default",
  "Modifier" varchar(30) COLLATE "pg_catalog"."default",
  "ModifyDate" timestamp(6),
  "ModifyID" int4
)
;

SELECT setval('"public"."sbm_stockpicking_id_seq"', 1001, true);



COMMENT ON TABLE  "public"."sbm_stock_picking" is '調撥單主檔' ;
COMMENT ON COLUMN "public"."sbm_stock_picking"."picking_id" IS 'ID';
COMMENT ON COLUMN "public"."sbm_stock_picking"."name" IS '調撥單單號';
COMMENT ON COLUMN "public"."sbm_stock_picking"."picking_date" IS '調撥日期';
COMMENT ON COLUMN "public"."sbm_stock_picking"."picking_type" IS '調撥類型';
COMMENT ON COLUMN "public"."sbm_stock_picking"."origin" IS '來源單號';
COMMENT ON COLUMN "public"."sbm_stock_picking"."location_source" IS '來源位置';
COMMENT ON COLUMN "public"."sbm_stock_picking"."location_destination" IS '目的位置';
COMMENT ON COLUMN "public"."sbm_stock_picking"."picking_owner" IS '作業人員';
COMMENT ON COLUMN "public"."sbm_stock_picking"."company_id" IS '事業單位';
COMMENT ON COLUMN "public"."sbm_stock_picking"."partner_company" IS '合作夥伴公司';
COMMENT ON COLUMN "public"."sbm_stock_picking"."partner_contact" IS '夥伴聯絡人';
COMMENT ON COLUMN "public"."sbm_stock_picking"."bu_logo" IS 'LOGO';



-----------------------------
-- Sequence structure for sbm_stockmove_id_seq
------------------------------
DROP SEQUENCE IF EXISTS "public"."sbm_stockmove_id_seq" cascade;
CREATE SEQUENCE "public"."sbm_stockmove_id_seq" 
INCREMENT 1
MINVALUE  1000
MAXVALUE 9223372036854775807
START 1000
CACHE 1;


-- ----------------------------
-- Table structure for sbm_stockmove
-- ----------------------------
DROP TABLE IF EXISTS "public"."sbm_stockmove" cascade;
CREATE TABLE "public"."sbm_stockmove" (
  "id" int4 NOT NULL DEFAULT nextval('sbm_stockmove_id_seq'::regclass),
  "picking_id" int4,
  "move_item" varchar(20),
  "prod_name" TEXT,
  "prod_num" float,
  "prod_uom" varchar(10),
  "price_unit" float,
  "price_sub" float,
  "prod_desc" varchar(300),
  "CreateDate" timestamp(6),
  "CreateID" int4,
  "Creator" varchar(30) COLLATE "pg_catalog"."default",
  "Modifier" varchar(30) COLLATE "pg_catalog"."default",
  "ModifyDate" timestamp(6),
  "ModifyID" int4
)
;

SELECT setval('"public"."sbm_stockmove_id_seq"', 1001, true);

COMMENT ON TABLE  "public"."sbm_stockmove" is '調撥明細檔' ;
COMMENT ON COLUMN "public"."sbm_stockmove"."id" IS 'ID';
COMMENT ON COLUMN "public"."sbm_stockmove"."picking_id" IS '主檔關聯';
COMMENT ON COLUMN "public"."sbm_stockmove"."move_item" IS '項次';
COMMENT ON COLUMN "public"."sbm_stockmove"."prod_name" IS '產品';
COMMENT ON COLUMN "public"."sbm_stockmove"."prod_num" IS '數量';
COMMENT ON COLUMN "public"."sbm_stockmove"."prod_uom" IS '單位';
COMMENT ON COLUMN "public"."sbm_stockmove"."price_unit" IS '單價';
COMMENT ON COLUMN "public"."sbm_stockmove"."price_sub" IS '小計';
COMMENT ON COLUMN "public"."sbm_stockmove"."prod_desc" IS '備註';


-----------------------------
-- Sequence structure for sbm_stockoutopen_id_seq
------------------------------
DROP SEQUENCE IF EXISTS "public"."sbm_stockoutopen_id_seq" cascade;
CREATE SEQUENCE "public"."sbm_stockoutopen_id_seq" 
INCREMENT 1
MINVALUE  1001
MAXVALUE 9223372036854775807
START 1001
CACHE 1;


-- ----------------------------
-- Table structure for sbm_stockoutopen
-- ----------------------------
DROP TABLE IF EXISTS "public"."sbm_stockoutopen" cascade;
CREATE TABLE "public"."sbm_stockoutopen" (
  "id" int4 NOT NULL DEFAULT nextval('sbm_stockoutopen_id_seq'::regclass),
  "so_id" int4,
  "picking_date" date,
  "CreateDate" timestamp(6),
  "CreateID" int4,
  "Creator" varchar(30) COLLATE "pg_catalog"."default",
  "Modifier" varchar(30) COLLATE "pg_catalog"."default",
  "ModifyDate" timestamp(6),
  "ModifyID" int4
)
;

alter table sbm_stockoutopen add column if not exists picking_date date ;

COMMENT ON TABLE  "public"."sbm_stockoutopen" is '出貨單建立' ;
COMMENT ON COLUMN "public"."sbm_stockoutopen"."id" IS 'ID';
COMMENT ON COLUMN "public"."sbm_stockoutopen"."so_id" IS '報價單號';
COMMENT ON COLUMN "public"."sbm_stockoutopen"."picking_date" IS '調撥日期';

-----------------------------
-- Sequence structure for sbm_stockno_id_seq
------------------------------
DROP SEQUENCE IF EXISTS "public"."sbm_rstockno_id_seq" cascade;
CREATE SEQUENCE "public"."sbm_stockno_id_seq" 
INCREMENT 1
MINVALUE  1000
MAXVALUE 9223372036854775807
START 1000
CACHE 1;

-- ----------------------------
-- Table structure for sbm_stockno
-- ----------------------------
DROP TABLE IF EXISTS "public"."sbm_stockno" cascade;
CREATE TABLE "public"."sbm_stockno" (
  "id" int4 NOT NULL DEFAULT nextval('sbm_stockno_id_seq'::regclass),
  "bu_id" int4,
  "picking_type" int4,
  "prefix_code" varchar(10),
  "stock_year" varchar,
  "stock_month" varchar,
  "seq" int4
  )
;























