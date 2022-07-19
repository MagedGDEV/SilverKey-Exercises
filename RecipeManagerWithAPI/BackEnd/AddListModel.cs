using System;

public class AddListModel
{
    public string ItemToAdd { get; set; }
    public int Position { get; set; }

    public AddListModel(string itemToAdd, int position)
    {
        this.ItemToAdd = itemToAdd;
        this.Position = position;
    }
}


