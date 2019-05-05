## JHilburn Fabric Manager 

### Task
Create a fabric management application using C# and optionally javascript that integrates with our fabric api. Features include but are not limited to:

* Display a list of available fabrics
* Add/Update fabrics
* Manage fabric inventory levels

Bonus points for any extra features you can come up with to make your application more functional.


### Notes

The basic page is a list view, listing all of the fabrics.  The list is paged (10 items per page) and sorted by Id ascending.  Each of the column headers is clickable, applying a sort by that column.  

Each row of the list is clickable, opening a modal for editing that fabric.  Inventory level editing is available here.  The row also includes a delete button to delete a particular fabric.

The create button opens a modal similar to the update modal and allows for creating a new fabric entry.

The search input provides a search by description which runs on keypress. Applying a search or sort returns the list to page 1 of the responses.
