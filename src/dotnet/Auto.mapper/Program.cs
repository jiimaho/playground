using Automapper;
using Automapper.Mapping;
using Automapper.PrintHelper;
using Domain;

var mapper = MapperFactory.CreateMapper();

var model = ModelFactory.GenerateCarIncident(); 

var dto = mapper.Map<CarIncidentDto>(model);

PrintHelper.Print(model, dto);

