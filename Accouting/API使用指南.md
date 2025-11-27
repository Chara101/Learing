# AccountAPI 使用指南
#### 請自己運行API，本人測試用路由 : https://localhost:7244/api/

| 目的  | http方法 | 請求 | 回傳 | 路由 |
| --- | ------ | ---- | ---- | --- |
| GetAllRecords | Get | 無 | 格式：json; 欄位：id, date, category_id, category, subCategory_id, subcategory, amount, comment; |
| GetRecord | Get | 選擇性傳入: id, date, category_id, subcategory_id, user_id, amount | 同GetAllRecords |
| GetRecordInRange | Get | 選擇性傳入成對的: id, date, category_id, subcategory_id, user_id, amount | 同GetAllRecords |
| GetTotals | Get | 
| GetTotalsBy | Get |
| GetTotalsInRange | Get |
| AddObject | Post |
| Renew | Put |
| Delete | Delete |
| AddCategory | Post |
| DeleteCategory | Delete |
| AddSubCategory | Post |
| DeleteSubCategory | Delete |
| GetAllCategories | Get |
| GetAllSubCategories | Get |
| GetAllCategoriesAndSub | Get |