﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Pokloni.ba.Model.Requests.Narudzba;
using Pokloni.ba.WebAPI.Database;
using Pokloni.ba.WebAPI.Exceptions;

namespace Pokloni.ba.WebAPI.Services.Narudzbe
{
    public class NarudzbaService : INarudzbaService
    {
        private readonly PokloniContext _db;
        private readonly IMapper _mapper;
        public NarudzbaService(PokloniContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public IEnumerable<NarudzbaVM> Get()
        {
            var temp = _db.Narudzba.Include(k=>k.Korisnik).Include(c=>c.Zaposlenik).Include(l=>l.Dostava).ToList();

            return _mapper.Map<IEnumerable<NarudzbaVM>>(temp);
        }

        public NarudzbaVM GetById(int id)
        {
            var temp = _db.Narudzba.Find(id) ?? throw new ServerException(Constants.NotFoundErrorMessage + id);

            return _mapper.Map<NarudzbaVM>(temp);
        }

        public NarudzbaVM Insert(NarudzbaVM request)
        {
            request.Dostava = null;
            request.Korisnik = null;
            request.Zaposlenik = null;

            var temp = _mapper.Map<Database.Narudzba>(request);

            var test = _db.Narudzba.Add(temp);
            _db.SaveChanges();

            request.NarudzbaId = test.Entity.NarudzbaId;
            return request;
        }

        public NarudzbaVM Update(NarudzbaVM request, int id)
        {
            request.Dostava = null;
            request.Korisnik = null;
            request.Zaposlenik = null;

            var temp = _db.Narudzba.Find(id) ?? throw new ServerException(Constants.NotFoundErrorMessage + id);

            _mapper.Map(request, temp);
            _db.Narudzba.Update(temp);
            _db.SaveChanges();

            return request;
        }

        public void Delete(int id)
        {
            var temp = _db.Narudzba.Find(id) ?? throw new ServerException(Constants.NotFoundErrorMessage + id);

            _db.Narudzba.Remove(temp);
            _db.SaveChanges();
        }
    }
}
