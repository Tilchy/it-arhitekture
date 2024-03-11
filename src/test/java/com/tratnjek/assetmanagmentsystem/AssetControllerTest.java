package com.tratnjek.assetmanagmentsystem;

import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.context.SpringBootTest;
import org.springframework.validation.BindingResult;

import java.math.BigDecimal;

import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.Mockito.mock;
import static org.mockito.Mockito.when;

@SpringBootTest
class AssetControllerTest {

    @Autowired
    AssetController assetController;

    AssetDto asset = null;

    @BeforeEach
    void createNewAsset() {
        var bindingResult = mock(BindingResult.class);
        when(bindingResult.hasErrors()).thenReturn(false);

        var assetDto = new AssetDto("Asset1", "Type1", "Status1", new BigDecimal("15.12"), new BigDecimal("43.13"));
        asset = (AssetDto) assetController.createAsset(assetDto,bindingResult).getBody();
    }

    @Test
    void getAllAssets() {
        var assets = assetController.getAllAssets();
        assertNotNull(assets);
        assertTrue(assets.iterator().hasNext());
    }

    @Test
    void getAssetById() {
        var assetById = assetController.getAssetById(Long.valueOf(1));
        assertNotNull(assetById);
        assertEquals(asset, assetById.getBody());
    }

    @Test
    void updateAsset() {
        var bindingResult = mock(BindingResult.class);
        when(bindingResult.hasErrors()).thenReturn(false);

        var assetDto = new AssetDto("Asset2", "Type2", "Status2", new BigDecimal("15.12"), new BigDecimal("43.13"));
        var updatedAsset = assetController.updateAsset(Long.valueOf(1), assetDto, bindingResult);
        assertNotNull(updatedAsset);
        assertEquals(assetDto, updatedAsset.getBody());
    }

    @Test
    void deleteAsset() {
        var deletedAsset = assetController.deleteAsset(Long.valueOf(1));

        assertNotNull(deletedAsset);
        assertEquals(204, deletedAsset.getStatusCodeValue());

    }
}