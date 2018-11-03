# Todo CQRS

## Scenarios

### Empty list can have item added

Given an empty Todo list<br/>
When I add a Todo for 'Buy cheese'<br/>
Then only that item is listed

### Empty list can have two items added

Given an empty Todo list<br/>
When I add Todos for 'Buy cheese' & 'Wash the car'<br/>
Then only those items are listed

### Item completion changes the list

Given a Todo list with items 'Buy cheese' & 'Wash the car'<br/>
When the first item is marked as complete<br/>
Then only those items are listed<br/>
And only the second item is listed as active

### Item incompletion changes the list

Given a Todo list with items 'Buy cheese' & 'Wash the car'<br/>
And the first item is completed<br/>
When the first item is marked as incomplete<br/>
Then only those items are listed<br/>
And both items are listed as active

### Complete items can be cleared

Given a Todo list with items 'Buy cheese' & 'Wash the car'<br/>
And the first item is completed<br/>
When 'Clear completed' is executed<br/>
Then only the second item is listed

### Incomplete items can be removed

Given a Todo list with a single item 'Buy cheese'<br/>
When the item is removed<br/>
Then nothing is listed

### Complete items can be removed

Given a Todo list with a single item 'Buy cheese'<br/>
And the item is completed<br/>
When the item is removed<br/>
Then nothing is listed

### Editing can change the text of an item

Given a Todo list with a single item 'Buy cheese'<br/>
When the item changed to 'Apply for 6-month tax extension'<br/>
Then only the revised item is listed
