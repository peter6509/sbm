drop function if exists bfins_sys_user() cascade ;
create or replace function bfins_sys_user() returns trigger as $BODY$
DECLARE
   cname varchar ;

BEGIN
     NEW."Enable" = 1 ;
      return NEW ;
END;$BODY$
LANGUAGE plpgsql;  



drop trigger if exists  bfins_sys_user on "Sys_User" ;
create trigger bfins_sys_user before insert on  "Sys_User"
   for each row execute procedure  bfins_sys_user();


drop function if exists bfins_saleorder() cascade ;
create or replace function bfins_saleorder() returns trigger as $BODY$
DECLARE
   cyear varchar ;
   cmonth varchar ;
   ncount int ;
   nseq int ;
   myres varchar ;
   buid int ;
   pcode varchar ;
   bankno varchar ;
   bankname varchar ;
   bankbno varchar ;
   bankbname varchar ;
   bankaccno varchar ;
   bankaccname varchar ;
BEGIN
   if NEW.company_id = 1 then
      pcode = 'LS' ;
      select bu_bankno,bu_bankname,bu_bankbno,bu_bankbname,bu_accno,bu_accname into bankno,bankname,bankbno,bankbname,bankaccno,bankaccname from sbm_bu where id = 1 ;
   elsif NEW.company_id = 2 then
      pcode = 'AS' ;
      select bu_bankno,bu_bankname,bu_bankbno,bu_bankbname,bu_accno,bu_accname into bankno,bankname,bankbno,bankbname,bankaccno,bankaccname from sbm_bu where id = 2 ;
   elsif NEW.company_id=3 then
      pcode = 'CS' ;
      select bu_bankno,bu_bankname,bu_bankbno,bu_bankbname,bu_accno,bu_accname into bankno,bankname,bankbno,bankbname,bankaccno,bankaccname from sbm_bu where id = 3 ;
   end if ; 
   NEW.bu_bank_info = concat(bankno,bankname,' ',bankbno,bankbname,' ','戶名:',bankaccname,'帳號:',bankaccno) ;
   select date_part('year',NEW.sale_date)::TEXT into cyear ;
   select lpad(date_part('month',NEW.sale_date)::TEXT,2,'0') into cmonth ;
   select count(*) into ncount from sbm_saleorderno where sale_year=cyear and sale_month=cmonth and bu_id=NEW.company_id;
   if ncount = 0 then
      nseq = 1 ;
      insert into sbm_saleorderno(sale_year,sale_month,seq,bu_id,prefix_code) values (cyear,cmonth,2,NEW.company_id,pcode) ;
   else
      select seq into nseq from sbm_saleorderno where sale_year=cyear and sale_month=cmonth and bu_id = NEW.company_id ;
      update sbm_saleorderno set seq = seq + 1 where sale_year=cyear and sale_month=cmonth and bu_id = NEW.company_id;
   end if ; 
   myres = concat(pcode,'-',cyear,cmonth,'-',lpad(nseq::TEXT,3,'0')) ;
   NEW.name = myres ;
   if NEW.sales is not null and NEW.sales_mobile is null then
      select "PhoneNo" into NEW.sales_mobile from "Sys_User" where "User_Id"=NEW.sales ;
   end if ; 
   if NEW.partner_contact is not null and NEW.partner_mobile is null then
      select mobile into NEW.partner_mobile from sbm_partner where id = NEW.partner_contact ;
   end if ;
   if NEW.sales is not null and NEW.sales_email is null then
      select "Email" into NEW.sales_email from "Sys_User" where "User_Id" = NEW.sales ;
   end if ;
   if NEW.partner_contact is not null and NEW.partner_email is null then
      select email into NEW.partner_email from sbm_partner where id = NEW.partner_contact ;
   end if ;
   if NEW.company_id is not null then
      select bu_logo,bu_invoice_stamp into NEW.bu_logo,NEW.bu_invoice_stamp from sbm_bu where id = NEW.company_id ;
   end if ;
   return NEW ;
END;$BODY$
LANGUAGE plpgsql;  



drop trigger if exists bfins_saleorder on sbm_sale_order ;
create trigger bfins_saleorder before insert on sbm_sale_order
   for each row execute procedure bfins_saleorder();



drop function if exists bfup_saleorder_bu() cascade ;
create or replace function bfup_saleorder_bu() returns trigger as $BODY$
DECLARE

BEGIN
  if NEW.company_id is not null then
      select bu_logo,bu_invoice_stamp into NEW.bu_logo,NEW.bu_invoice_stamp from sbm_bu where id = NEW.company_id ;
  end if ;
  return NEW ;

END;$BODY$
LANGUAGE plpgsql;



drop trigger if exists bfup_saleorder_bu on sbm_sale_order ;
create trigger bfup_saleorder_bu before update of company_id on sbm_sale_order
   for each row execute procedure bfup_saleorder_bu();



drop function if exists bfup_discount() cascade ;
create or replace function bfup_discount() returns trigger as $BODY$
DECLARE
   ncount int ; 
BEGIN
    if NEW.discount_amount is not null then
       NEW.tax = round(NEW.discount_amount::numeric * 0.05::numeric,0) ;
       NEW.tot_amount = NEW.discount_amount + NEW.tax ;
    end if ;
    return NEW ;
END;$BODY$
LANGUAGE plpgsql;  



drop trigger if exists bfup_discount on sbm_sale_order ;
create trigger bfup_discount before update of discount_amount on sbm_sale_order
   for each row execute procedure bfup_discount();



drop function if exists bfins_partner() cascade ;
create or replace function bfins_partner() returns trigger as $BODY$
DECLARE
   caddress varchar ;
   cvat varchar ;
BEGIN
  if NEW.partner_type = 2 and NEW.parent_id is not null then
     select address,vat into caddress,cvat from sbm_partner where id = NEW.parent_id ;
     if NEW.vat is null then
        NEW.vat = cvat ;
     end if ;
     if NEW.address is null then
        NEW.address = caddress ;
     end if ;
  end if ;
  return NEW ;
END;$BODY$
LANGUAGE plpgsql;  



drop trigger if exists bfins_partner on sbm_partner ;
create trigger bfins_partner before insert on sbm_partner
   for each row execute procedure bfins_partner();



drop function if exists bfins_sbm_require_purchase() cascade ;
create or replace function  bfins_sbm_require_purchase() returns trigger as $BODY$
DECLARE
   cname varchar ;
   buid int ;
   pcode varchar ;
   ncount int ;
   cyear varchar ;
   cmonth varchar ;
   nseq int ;
BEGIN
   
   if NEW.sale_id is not null then
      select name,company_id into cname,buid from sbm_sale_order where sale_id = NEW.sale_id ;
      if buid = 1 then
         pcode = 'LR' ;
      elsif buid = 2 then
         pcode = 'AR' ;
      elsif buid = 3 then
         pcode = 'CR' ;
      end if ;
      select date_part('year',current_date)::TEXT into cyear ;
      select lpad(date_part('month',current_date)::TEXT,2,'0') into cmonth ;
      select count(*) into ncount from sbm_reqno where req_year=cyear and req_month=cmonth and bu_id=buid ;
      if ncount = 0 then
         insert into sbm_reqno(req_year,req_month,seq,bu_id,prefix_code) values (cyear,cmonth,2,buid,pcode) ;
         NEW.name = concat(pcode,'-',cyear,cmonth,'-001') ;
      else
         select seq into nseq from sbm_reqno where req_year=cyear and req_month=cmonth and bu_id=buid ;
         NEW.name = concat(pcode,'-',cyear,cmonth,'-',lpad(nseq::TEXT,3,'0')) ;
         update sbm_reqno set seq = seq + 1 where req_year=cyear and req_month=cmonth and bu_id=buid ;
      end if ;
   end if ;
   return NEW ;
END;$BODY$
LANGUAGE plpgsql;  



drop trigger if exists  bfins_sbm_require_purchase on sbm_require_purchase ;
create trigger bfins_sbm_require_purchase before insert on  sbm_require_purchase
   for each row execute procedure  bfins_sbm_require_purchase();




drop function if exists afins_sbm_reqopen() cascade ;
create or replace function  afins_sbm_reqopen() returns trigger as $BODY$
DECLARE
   so_cur refcursor ; 
   so_rec record ;
   ncount int ;
   nreqid int ;
   enduserid int ;
BEGIN
   select count(*) into ncount from sbm_require_purchase where sale_id=NEW.so_id and partner_company=NEW.partner_name ;
   select partner_company into enduserid from sbm_sale_order where sale_id = NEW.so_id ;
   if ncount = 0 then
    insert into sbm_require_purchase(sale_id,partner_company,partner_contact,end_user,"CreateID","Creator") values     
     (NEW.so_id,NEW.partner_name,NEW.partner_contact,enduserid,NEW."CreateID",NEW."Creator") ;
    select max(req_id) into nreqid from sbm_require_purchase ;
      open so_cur for select * from sbm_sale_order_line where sale_id=NEW.so_id order by sale_item ;
      loop
        fetch so_cur into so_rec ;
        exit when not found ;
        select count(*) into ncount from sbm_require_purchase_line where req_id=nreqid and prod_name=so_rec.prod_name and req_item = so_rec.sale_item ;
        if ncount = 0 then
           insert into sbm_require_purchase_line(req_id,req_item,prod_name,prod_num,prod_uom,prod_desc,"CreateID","Creator") values          
            (nreqid,so_rec.sale_item,so_rec.prod_name,so_rec.prod_num,so_rec.prod_uom,so_rec.prod_desc,NEW."CreateID",NEW."Creator") ;
        end if ; 
      end loop ;
      close so_cur ;
      
   end if ;
   return NEW ;
END;$BODY$
LANGUAGE plpgsql;  



drop trigger if exists afins_sbm_reqopen on sbm_reqopen ;
create trigger afins_sbm_reqopen after insert on  sbm_reqopen
   for each row execute procedure  afins_sbm_reqopen();


drop function if exists bfup_reqdiscount() cascade ;
create or replace function bfup_reqdiscount() returns trigger as $BODY$
DECLARE
   ncount int ; 
BEGIN
    if NEW.discount_amount is not null then
       NEW.tax = round(NEW.discount_amount::numeric * 0.05::numeric,0) ;
       NEW.tot_amount = NEW.discount_amount + NEW.tax ;
    end if ;
    return NEW ;
END;$BODY$
LANGUAGE plpgsql;  



drop trigger if exists bfup_reqdiscount on sbm_require_purchase ;
create trigger bfup_reqdiscount before update of discount_amount on sbm_require_purchase
   for each row execute procedure bfup_reqdiscount();


drop function if exists afins_sbm_stockoutopen() cascade ;
create or replace function  afins_sbm_stockoutopen() returns trigger as $BODY$
DECLARE
   so_cur refcursor ; 
   so_rec record ;
   ncount int ;
   pickingid int ;
   enduserid int ;
   enduserid1 int;
   soname varchar ;
   buid int ;
   buname varchar ;
   partnername varchar ;
   bulogo varchar(200);
BEGIN
   select name,partner_company,partner_contact,company_id into soname,enduserid,enduserid1,buid from sbm_sale_order where sale_id=NEW.so_id ;
   select bu_name,bu_logo into buname,bulogo from sbm_bu where id = buid ;
   select name into partnername from sbm_partner where id = enduserid ;
   select count(*) into ncount from sbm_stock_picking where origin=soname  ;
   if ncount = 0 then
    insert into     sbm_stock_picking(picking_date,picking_type,origin,location_source,location_destination,picking_owner,company_id,partner_company,partner_contact,bu_logo,"CreateID","Creator")
 values (NEW.picking_date,2,soname,buname,partnername,NEW."CreateID",buid,enduserid,enduserid1,bulogo,NEW."CreateID",NEW."Creator") ;
    select max(picking_id) into pickingid from sbm_stock_picking ;
      open so_cur for select * from sbm_sale_order_line where sale_id=NEW.so_id order by sale_item ;
      loop
        fetch so_cur into so_rec ;
        exit when not found ;
        select count(*) into ncount from sbm_stockmove where picking_id=pickingid and prod_name=so_rec.prod_name and move_item = so_rec.sale_item ;
        if ncount = 0 then
           insert into sbm_stockmove(picking_id,move_item,prod_name,prod_num,prod_uom,price_unit,price_sub,prod_desc,"CreateID","Creator") values          
            (pickingid,so_rec.sale_item,so_rec.prod_name,so_rec.prod_num,so_rec.prod_uom,so_rec.price_unit,so_rec.price_sub,so_rec.prod_desc,NEW."CreateID",NEW."Creator") ;
        end if ; 
      end loop ;
      close so_cur ;
      
   end if ;
   return NEW ;
END;$BODY$
LANGUAGE plpgsql;  



drop trigger if exists afins_sbm_stockoutopen on sbm_stockoutopen ;
create trigger afins_sbm_stockoutopen after insert on sbm_stockoutopen
   for each row execute procedure  afins_sbm_stockoutopen();


drop function if exists bfins_sbm_stockpicking() cascade ;
create or replace function  bfins_sbm_stockpicking() returns trigger as $BODY$
DECLARE
  pcode varchar ;
  pcode1 varchar ;
  prefixcode varchar ;
  cyear varchar ;
  cmonth varchar ;
  pickingdate date ;
  ncount int ;
  nseq int ;
  bulogo varchar ;
BEGIN
  if NEW.company_id is not null and NEW.picking_type is not null then
     if NEW.company_id = 1 then
        pcode = 'LS' ;
     elsif NEW.company_id = 2 then
        pcode = 'AS' ;
     elsif NEW.company_id =3 then
        pcode = 'CS' ;
     end if ;
     if NEW.picking_type = 1 then
        pcode1 = 'I' ;
     elsif NEW.picking_type = 2 then
        pcode1 = 'O' ;
     end if ;
     prefixcode = concat(pcode,pcode1) ;
  end if ;
  if NEW.picking_date is not null then
     pickingdate = NEW.picking_date ;
  else
     pickingdate = current_date ;
  end if ;
  if NEW.company_id is not null and NEW.bu_logo is null then
     select bu_logo into NEW.bu_logo from sbm_bu where id = NEW.company_id ;
  end if ;
  select date_part('year',pickingdate):: TEXT into cyear ;
  select lpad(date_part('month',pickingdate)::TEXT,2,'0') into cmonth ;
  select count(*) into ncount from sbm_stockno where bu_id=NEW.company_id and picking_type=NEW.picking_type and stock_year=cyear and stock_month=cmonth ;
  if ncount = 0 then
     insert into sbm_stockno(bu_id,picking_type,prefix_code,stock_year,stock_month,seq) values (NEW.company_id,NEW.picking_type,prefixcode,cyear,cmonth,2) ;
     NEW.name = concat(prefixcode,'-',cyear,cmonth,'-001') ;
  else
     select seq into nseq from sbm_stockno where bu_id=NEW.company_id and picking_type=NEW.picking_type and stock_year=cyear and stock_month=cmonth ;
     update sbm_stockno set seq = seq + 1 where bu_id=NEW.company_id and picking_type=NEW.picking_type and stock_year=cyear and stock_month=cmonth ;
     NEW.name = concat(prefixcode,'-',cyear,cmonth,'-',lpad(nseq::TEXT,3,'0')) ;
  end if ;
  return NEW ;
END;$BODY$
LANGUAGE plpgsql;  



drop trigger if exists bfins_sbm_stockpicking on sbm_stock_picking ;
create trigger bfins_sbm_stockpicking before insert on sbm_stock_picking
   for each row execute procedure  bfins_sbm_stockpicking();


