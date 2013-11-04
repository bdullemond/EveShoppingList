/*
Using the data export from CCP found here:  http://community.eveonline.com/community/fansites/toolkit/
This query extracts a dictionary of type names (modules, ships, charges) and their
parent market group name. I could have gotten all group names in the heirarchy but
I felt it was too messy when all I want is high level grouping and sorting.
The output of this query is what is found in items.xml
*/
declare 
   @marketGroupName varchar(255),
   @ParentGroupID int,
   @newMarketGroupName varchar(255),
   @typeName varchar(255) 
   
declare @types table (GroupName varchar(255), TypeName varchar(255))

declare typeCursor cursor for
   select mg.marketGroupName, mg.parentGroupID, t.typeName
   from invTypes t
   inner join invGroups g
                     on g.groupID = t.groupID
                    and g.categoryID in (6, 7, 8, 18, 32, 63) -- Ship, Module, Charge, Drone, Subsystem, Special Edition Assets
   inner join invMarketGroups mg
                           on mg.marketGroupID = t.marketGroupID

open typeCursor

fetch next from typeCursor into @marketGroupName, @ParentGroupID, @typeName

while (@@FETCH_STATUS = 0)
begin

   while (@ParentGroupID is not null)
   begin
   
      select 
         @newMarketGroupName = marketGroupName,
         @ParentGroupID = parentGroupID
      from invMarketGroups
      where marketGroupID = @ParentGroupID
      
      if @ParentGroupID is not null
      begin
         select @marketGroupName = @newMarketGroupName
      end
    
   end
   
   insert @types (GroupName, TypeName)
   values (@marketGroupName, @typeName)
   
   fetch next from typeCursor into @marketGroupName, @ParentGroupID, @typeName
end

close typeCursor
deallocate typeCursor

select GroupName, TypeName
from @types
FOR XML RAW ('Item'), ROOT ('Items')

