{TRANSAKCIJA - prepare,commit,rollback

	ROMENITI->	book 			- naziv objekta klase
				RetrieveOneBook - nazivi metoda repository-a
				...

	public void Commit()
	{
		if (prepBookID == null)
		{
			return;
		}
	
		Book book = repository.RetrieveOneBook(prepBookID+"prep");
	
		if (book != null)
		{
	
			repository.RemoveBook(book);
			string updateBookID = book.RowKey.Remove(book.RowKey.IndexOf("prep"));
			Book updateBook = repository.RetrieveOneBook(updateBookID);
	
			updateBook.Count = book.Count;
	
			repository.ReplaceBook(updateBook);
		}
	}
	public bool Prepare()
	{
		if(prepBookID == null)
		{
			return false;
		}
	
		Book book = repository.RetrieveOneBook(prepBookID);
	
		if(book != null && book.Count - prepCount >=0)
		{
			Book prepBook = new Book(prepBookID + "prep")
			{
				Price = book.Price,
				Count = book.Count - prepCount
				
			};
	
			repository.AddBook(prepBook);
	
			return true;
		}
	
		return false;
	}
	
	public void Rollback()
	{
		if(prepBookID == null)
		{
				return;
		}
	
		Book book = repository.RetrieveOneBook(prepBookID + "prep");
		
		if(book != null)
		{
			repository.RemoveBook(book);
		}
	}
}