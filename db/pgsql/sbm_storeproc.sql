drop function if exists gen_reqline(soid int,reqid int) cascade ;
create or replace function gen_reqline(soid int,reqid int) returns void as $BODY$
DECLARE
  so_cur refcursor ;
  so_rec record ;
  ncount int ;
  cid int ;
  ccreator varchar ;
BEGIN
   if soid is not null and reqid is not null then
      select "CreateID","Creator" into cid,ccreator from sbm_require_purchase where id = reqid ;
      open so_cur for select * from sbm_sale_order_line where sale_id=soid order by sale_item ;
      loop
        fetch so_cur into so_rec ;
        exit when not found ;
        select count(*) into ncount from sbm_require_purchase_line where req_id=reqid and prod_name=so_rec.prod_name  ;
        if ncount = 0 then
           insert into sbm_require_purchase_line(req_id,req_item,prod_name,prod_num,prod_uom,prod_desc,price_unit,price_sub,"CreateID","Creator") values                       
           (reqid,so_rec.sale_item,so_rec.prod_name,so_rec.prod_num,so_rec.prod_uom,so_rec.prod_desc,0,0,cid,ccreator) ;
        end if ; 
      end loop ;
      close so_cur ;
    end if ;   

END;$BODY$
LANGUAGE plpgsql ;



