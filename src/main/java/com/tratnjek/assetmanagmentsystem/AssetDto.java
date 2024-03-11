package com.tratnjek.assetmanagmentsystem;

import jakarta.validation.constraints.DecimalMax;
import jakarta.validation.constraints.DecimalMin;
import jakarta.validation.constraints.NotBlank;
import jakarta.validation.constraints.NotNull;

import java.math.BigDecimal;

public record AssetDto(
        @NotBlank(message = "Name is mandatory")
        String name,
        @NotBlank(message = "Type is mandatory")
        String type,
        @NotBlank(message = "Status is mandatory")
        String status,
        @NotNull(message = "Latitude is mandatory")
        @DecimalMin(value = "-90", message = "Latitude should be between -90 and 90")
        @DecimalMax(value = "90", message = "Latitude should be between -90 and 90")
        BigDecimal latitude,
        @NotNull(message = "Longitude is mandatory")
        @DecimalMin(value = "-180", message = "Longitude should be between -180 and 180")
        @DecimalMax(value = "180", message = "Longitude should be between -180 and 180")
        BigDecimal longitude) {

}
