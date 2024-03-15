package com.tratnjek.assetmanagmentsystem;

import jakarta.persistence.Entity;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.Id;
import jakarta.validation.constraints.DecimalMax;
import jakarta.validation.constraints.DecimalMin;
import jakarta.validation.constraints.NotBlank;
import jakarta.validation.constraints.NotNull;
import lombok.Data;
import lombok.NoArgsConstructor;

import java.math.BigDecimal;

@Entity
@Data
@NoArgsConstructor
public class Asset{

    @Id
    @GeneratedValue
    private Long id;
    @NotBlank(message = "Name is mandatory")
    private String name;
    @NotBlank(message = "Type is mandatory")
    private String type;
    @NotBlank(message = "Status is mandatory")
    private String status;
    @NotNull(message = "Latitude is mandatory")
    @DecimalMin(value = "-90", message = "Latitude should be between -90 and 90")
    @DecimalMax(value = "90", message = "Latitude should be between -90 and 90")
    private BigDecimal latitude;
    @NotNull(message = "Longitude is mandatory")
    @DecimalMin(value = "-180", message = "Longitude should be between -180 and 180")
    @DecimalMax(value = "180", message = "Longitude should be between -180 and 180")
    private BigDecimal longitude;

    public Asset(String name, String type, String status, BigDecimal latitude, BigDecimal longitude) {
        this.name = name;
        this.type = type;
        this.status = status;
        this.latitude = latitude;
        this.longitude = longitude;
    }

    public AssetDto toDto(){
        return new AssetDto(name, type, status, latitude, longitude);
    }

}
