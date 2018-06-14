{ VIEW
	{ FORMA ZA UNOS MODELA
		<script src="@Url.Content("~/Scripts/jquery.validate.min.js")"
			type="text/javascript"></script>
		<script src="@Url.Content("~/Scripts/jquery.validate.unobtrusive.min.js")"
				type="text/javascript"></script>
		@using (Html.BeginForm("AddEntity", "Home", FormMethod.Post, new
		{
			enctype =
		"multipart/form-data"
		}))
		{
			@Html.ValidationSummary(true)
			<fieldset>
				<legend>Film</legend>
				
				<div class="editor-label">
					@Html.LabelFor(model => model.Naziv)
				</div>
				<div class="editor-field">
					@Html.EditorFor(model => model.Naziv)
					@Html.ValidationMessageFor(model => model.Naziv)
				</div>
			   
				<input type="file" id="filmPhoto" name="file" />
				<p>
					<input type="submit" value="Create" />

				</p>
			</fieldset>
		}
	}

	{FORMA ZA UNOS PROSTIH TIPOVA
		<form action="/Home/FindFilm">
			<label>Vrsta proizvoda</label>
			<input type="text" name="naziv" />
			<button type="submit">Pronadji</button>
		</form>
	}

	{ POZIV AKCIJE KONTROLERA PREKO LINKA
		<div>
			@Html.ActionLink("Back to List", "Index")
		</div>
	}

	{COMBO BOX
		<select name="stanje">
			<option value="otvoreno">Otvoreno</option>
			<option value="zatvoreno">Zatvoreno</option>
		</select>
	}

	{TABELA SA POLJIMA MODELA I LINKOVIMA ZA ADD/EDIT/DELETE
		@model IEnumerable<StudentsService_Data.Student>
		@{
			ViewBag.Title = "Index";
		}
		<h2>
			Index
		</h2>
		<p>
			@Html.ActionLink("Create New", "Create")
		</p>
		<table>
			<tr>
				<th>
					Index
				</th>
				<th>
					Name
				</th>
				<th>
					LastName
				</th>
				<th>
					Photo
				</th>
				<th>
				</th>
			</tr>
			@foreach (var item in Model)
			{
				<tr>
					<td>
						@Html.DisplayFor(modelItem => item.RowKey)
					</td>
					<td>
						@Html.DisplayFor(modelItem => item.Name)
					</td>
					<td>
						@Html.DisplayFor(modelItem => item.LastName)
					</td>
					<td>
						<img src="@item.ThumbnailUrl" />
					</td>
					<td>
					  
						@Html.ActionLink("Edit", "Edit", new {  id=item.RowKey })
						@Html.ActionLink("Details", "Details", new {  id=item.RowKey })
						@Html.ActionLink("Delete", "Delete", new { id = item.RowKey  })
					</td>
				</tr>
			}
		</table>
	}
	
	{ISPIS SVIH STRINGOVA IZ LISTE POSLATE KROZ MODEL
		@{
			foreach (string s in Model)
			{
				<h3>@s</h3>
			}
		}
	}
}

{CONTROLLER
	
	{AKCIJA FORME ZA DODAVANJE OBJEKTA KLASE
		[HttpPost]
        public ActionResult AddEntity(string Naziv, HttpPostedFileBase file)
        {
            try
            {
                blobHelper.UploadFileToBlob(Naziv, file);
                queueHelper.AddToQueue(Naziv);
                return View("Index");
                
            }
            catch
            {
                return View("Greska");
            }
          
        }
	}
	
	{PREUZIMANJE SVIH PORUKA U REDU
		string s;
            while ((s = queueHelper2.GetFromQueue()) != null)
                ViewBag.Porudzbine += s;

            return View();
	}

	{POVRATAK NA AKCIJU(BOLJE OD RETURN VIEW() JER MENJA URL KOJI KORISNIK VIDI)
		return RedirectToAction("Index", repo.RetrieveAllStudents());
	}
	
	{LISTA STRINGOVA OD STRINGA UPISANOG U BLOB SA : KAO SPLITEROM
		List<string> recenice = blobHelper.DownloadStringFromBlob("recenice").Split(':').ToList();
	}
}





