﻿using System;
using System.Threading.Tasks;
using ZFood.Persistence.API;

namespace ZFood.Core.Validators.Impl
{
    class DeleteRestaurantValidator : IValidator<string>
    {
        private readonly IRestaurantRepository restaurantRepository;

        public DeleteRestaurantValidator(IRestaurantRepository restaurantRepository)
        {
            this.restaurantRepository = restaurantRepository;
        }

        public async Task<ValidatorResult> Validate(string id)
        {
            var validationResult = new ValidatorResult();
            if (string.IsNullOrWhiteSpace(id))
            {
                validationResult.Exception = new ArgumentNullException(nameof(id));
                return validationResult;
            }
            return await Task.FromResult(validationResult);
        }
    }
}
