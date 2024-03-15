package com.tratnjek.assetmanagmentsystem;

import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.context.SpringBootTest;
import org.springframework.test.context.ActiveProfiles;
import org.springframework.validation.BindingResult;

import java.math.BigDecimal;

import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.Mockito.mock;
import static org.mockito.Mockito.when;

@SpringBootTest
@ActiveProfiles("h2")
class AssetControllerTest {

    @Autowired
    AssetController assetController;

    @Autowired
    AssetRepository assetRepository;

    Asset createdAsset = null;

    @BeforeEach
    void createNewAsset() {
        var bindingResult = mock(BindingResult.class);
        when(bindingResult.hasErrors()).thenReturn(false);

        assetRepository.deleteAll();

        var asset = new Asset("Asset1", "Type1", "Status1", new BigDecimal("15.12"), new BigDecimal("43.13"));

        createdAsset = assetRepository.save(asset);

    }

    @Test
    void getAllAssets() {
        var assets = assetController.getAllAssets();
        assertNotNull(assets);
        assertTrue(assets.iterator().hasNext());
    }

    @Test
    void getAssetById() {

        var assetById = assetController.getAssetById(Long.valueOf(createdAsset.getId()));
        assertNotNull(assetById);
        assertEquals(createdAsset.toDto(), assetById.getBody());
    }

    @Test
    void updateAsset() {
        var bindingResult = mock(BindingResult.class);
        when(bindingResult.hasErrors()).thenReturn(false);

        var assetDto = new AssetDto("Asset2", "Type2", "Status2", new BigDecimal("15.12"), new BigDecimal("43.13"));
        var updatedAsset = assetController.updateAsset(Long.valueOf(createdAsset.getId()), assetDto, bindingResult);
        assertNotNull(updatedAsset);
        assertEquals(assetDto, updatedAsset.getBody());
    }

    @Test
    void deleteAsset() {
        var deletedAsset = assetController.deleteAsset(Long.valueOf(createdAsset.getId()));

        assertNotNull(deletedAsset);
        assertEquals(204, deletedAsset.getStatusCodeValue());

    }
}