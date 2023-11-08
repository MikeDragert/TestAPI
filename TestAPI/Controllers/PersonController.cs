using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using TestAPI.DAL;
using TestAPI.Model;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private PersonContext _dbContext;

        public PersonController(PersonContext dbContext) {
            _dbContext = dbContext;
        }

        // GET: api/<PersonController>
        [HttpGet("GetPersonList")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Person>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Get() {  //return list of all all people                      
            return getHttpResponse(_dbContext.Persons);
        }

        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Person))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        private ActionResult getHttpResponse(IEnumerable<Person> persons) {
            return persons == null || persons.Count() == 0 ? NotFound() : Ok(persons);
        }

        // GET api/<PersonController>/5
        [HttpGet("GetPersonById")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Person))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Get(int id) {
            //return getPersonById(id);
            if (isIdBad(id)) return getHttpBadResponse();
            return getHttpResponse(getPersonById(id));
        }

        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Person))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        private ActionResult getHttpResponse(Person person) {
            return isPersonFromError(person) ? NotFound() : Ok(person);
        }

        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        private ActionResult getHttpBadResponse() {
            return BadRequest();
        }        

        private bool isIdBad(int id) {
            return !isIdValid(id);
        }

        private bool isIdValid(int id) {
            return ((id != null) && (id > 0));
        }

        private Person getPersonById(int id) {
            Person person = null;
            try {
                person = _dbContext.Persons
                                    .Where(person => person.Id == id)
                                    .First<Person>();
            }
            catch (System.InvalidOperationException ex) {
                person = errorHandler();
            }
            return person;
        }

        // GET api/<PersonController>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Person))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet("GetPersonByUsername")]
        public ActionResult Get(string username) {
            if (isUsernameBad(username)) return getHttpBadResponse();
            return getHttpResponse(getPersonByUsername(username));
        }

        private bool isUsernameBad(string username) {
            return (!isUsernameValid(username));
        }

        private bool isUsernameValid(string username) {
            return (username != null && username.Length > 0);
        }

        private Person getPersonByUsername(string username) {
            Person person = null;
            try {
                person = _dbContext.Persons
                                    .Where(person => person.Username.ToLower() == username.ToLower())
                                    .First<Person>();
            }
            catch (System.InvalidOperationException ex) {
                person = errorHandler();
            }
            return person;
        }

        // GET api/<PersonController>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Person))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet("GetPersonByFirstLastName")]
        public ActionResult Get(string firstName, string lastName) {
            if (isFirstLastNameBad(firstName, lastName)) return getHttpBadResponse();
            return getHttpResponse(getPersonByFirstLastName(firstName, lastName));
        }

        private bool isFirstLastNameBad(string firstName, string lastName) {
            return (!isFirstLastNameValid(firstName, lastName));
        }

        private bool isFirstLastNameValid(string firstName, string lastName ) {
            return (firstName != null && firstName.Length > 0 &&
                    lastName != null && lastName.Length > 0);
        }

        private Person getPersonByFirstLastName(string firstName, string lastName){
            Person person = null;
            try {
                person = _dbContext.Persons
                                    .Where(person => person.FirstName.ToLower() == firstName.ToLower() &&
                                                     person.LastName.ToLower() == lastName.ToLower())
                                    .First<Person>();
            }
            catch (System.InvalidOperationException ex) {
                person = errorHandler();
            }
            return person;
        }

        // POST api/<PersonController>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Person))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult Post([FromBody] Person person) {           
            if (arePersonNamesBad(person)) return getHttpBadResponse();
            if (doesPersonExistByNames(person)) return getHttpBadResponse();
            Person dbPerson = addAndSavePersonToDb(person);
            return getHttpCreatedResponse(person, "GetPersonById");
        }

        private Person addAndSavePersonToDb(Person person) {
            try {
                _dbContext.Persons.Add(person);
                _dbContext.SaveChanges();
                return person;
            }
            catch (Exception ex) {
                return errorHandler();
            }
        }

        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Person))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        private ActionResult getHttpCreatedResponse(Person person, string getFunction) {
            if (isPersonFromError(person)) return getHttpBadResponse();
            string viewRouteName = @"/api/person/GetPersonById?id=" + person.Id.ToString();
            Uri newUri = new Uri(viewRouteName, UriKind.Relative);
            return Created(newUri, person); ;
        }

        private bool arePersonNamesBad(Person person) {
            return (!arePersonNamesValid(person));
        }
        private bool arePersonNamesValid(Person person) {
            return (isUsernameValid(person.Username) && (isFirstLastNameValid(person.FirstName, person.LastName)));
        }

        private bool doesPersonNotExistByNames(Person personToCompareTo) {
            return (!doesPersonExistByNames(personToCompareTo));
        }
        private bool doesPersonExistByNames(Person personToCompareTo) {
            Person person = getPersonByAnyName(personToCompareTo);
            return isPersonValid(person);
        }

        private Person getPersonByAnyName(Person personToCompareTo) {
            Person person = null;
            try {
                person = _dbContext.Persons
                                    .Where(person => ((person.Username.ToLower() == personToCompareTo.Username.ToLower()) ||
                                                      ((person.FirstName.ToLower() == personToCompareTo.FirstName.ToLower()) &&
                                                       (person.LastName.ToLower() == personToCompareTo.LastName.ToLower())))
                                    )
                                    .First<Person>();
            }
            catch (System.InvalidOperationException ex) {
                person = errorHandler();
            }
            return person;
        }

        // PUT api/<PersonController>/5
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Person))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult Put(int id, [FromBody] Person person) {
            if (isIdBad(id)) return getHttpBadResponse();
            Person dbPerson = getPersonById(person.Id);
            dbPerson = checkPersonAndSaveToDB(dbPerson, person);
            return (getHttpResponse(dbPerson));
        }

        private Person checkPersonAndSaveToDB(Person dbPerson, Person newPerson) {
            if (isPersonValid(dbPerson))  {
                dbPerson.Copy(newPerson);
                _dbContext.SaveChanges();
            }
            return dbPerson;
        }

        [HttpPut("IncrementById")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Person))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult Put(int id) {
            if (isIdBad(id)) return getHttpBadResponse();
            Person dbPerson = getPersonById(id);
            dbPerson = checkPersonAndIncrementCounteroDB(dbPerson);
            return (getHttpResponse(dbPerson));
        }

        private Person checkPersonAndIncrementCounteroDB(Person dbPerson) {
            if (isPersonValid(dbPerson)) {
                dbPerson.IncrementCounter();
                _dbContext.SaveChanges();
            }
            return dbPerson;
        }

        // DELETE api/<PersonController>/5
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult Delete(int id){
            if (isIdBad(id)) return getHttpBadResponse();
            Person dbPerson = getPersonById(id);
            if (isPersonFromError(dbPerson)) return NotFound();
            dbPerson = checkPersonAndDeleteFromDB(dbPerson);
            return (getHttpDeleteResponse(dbPerson));
        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        private ActionResult getHttpDeleteResponse(Person person) {
            return isPersonValid(person) ? BadRequest() : NoContent();
        }

        private Person checkPersonAndDeleteFromDB(Person dbPerson) {
            if (isPersonValid(dbPerson))  {
                _dbContext.Remove(dbPerson);
                _dbContext.SaveChanges();
                return null;
            }
            return dbPerson;
        }

        private Person errorHandler() {
            return null;        //todo:  is this how we really want to do it?  It works for now
        }

        private bool isPersonValid(Person person) {
            return (!isPersonFromError(person));
        }

        private bool isPersonFromError(Person person) {
            return person == null;
        }
    }
}
