package com.tratnjek.assetmanagmentsystem;

import jakarta.validation.Valid;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.context.support.DefaultMessageSourceResolvable;
import org.springframework.http.ResponseEntity;
import org.springframework.validation.BindingResult;
import org.springframework.web.bind.annotation.*;

import java.util.logging.Logger;

@CrossOrigin
@RestController
public class AssetController {

    private static final Logger _log = Logger.getLogger(AssetController.class.getName());

    @Autowired
    private AssetRepository assetRepository;

    @GetMapping("/assets")
    public @ResponseBody  Iterable<Asset> getAllAssets() {
        _log.info("Fetching all assets");
        return assetRepository.findAll();
    }

    @GetMapping("/assets/{id}")
    public ResponseEntity<AssetDto> getAssetById(@PathVariable("id") Long id) {
        _log.info("Fetching asset with id: " + id);

        var asset = assetRepository.findById(id);

        if (asset.isEmpty()) {
            _log.info("No asset found with id: " + id);
            return ResponseEntity.notFound().build();
        }

        return ResponseEntity.ok(asset.get().toDto());
    }

    @PostMapping("/assets")
    public ResponseEntity<?> createAsset(@Valid  @RequestBody AssetDto assetDto, BindingResult bindingResult){
        _log.info("Creating asset: " + assetDto);

        if(bindingResult.hasErrors()){
            var errors = bindingResult.getAllErrors()
                    .stream()
                    .map(DefaultMessageSourceResolvable::getDefaultMessage)
                    .toList();

            _log.info("Validation failed for asset: " + assetDto + " with errors: " + errors);

            return ResponseEntity.badRequest().body(errors);
        }

        var asset = new Asset(assetDto.name(), assetDto.type(), assetDto.status(), assetDto.latitude(), assetDto.longitude());
        assetRepository.save(asset);
        return ResponseEntity.ok(asset.toDto());
    }

    @PutMapping("/assets/{id}")
public ResponseEntity<?> updateAsset(@PathVariable("id") Long id, @Valid @RequestBody AssetDto assetDto, BindingResult bindingResult){
        _log.info("Updating asset with id: " + id + " with data: " + assetDto);

        if(bindingResult.hasErrors()){
            var errors = bindingResult.getAllErrors()
                    .stream()
                    .map(DefaultMessageSourceResolvable::getDefaultMessage)
                    .toList();

            _log.info("Validation failed for asset: " + assetDto + " with errors: " + errors);
        }

        var asset = assetRepository.findById(id);

        if (asset.isEmpty()) {
            _log.info("No asset found with id: " + id);
            return ResponseEntity.notFound().build();
        }

        var assetToUpdate = asset.get();
        assetToUpdate.setName(assetDto.name());
        assetToUpdate.setType(assetDto.type());
        assetToUpdate.setStatus(assetDto.status());
        assetToUpdate.setLatitude(assetDto.latitude());
        assetToUpdate.setLongitude(assetDto.longitude());
        assetRepository.save(assetToUpdate);
        return ResponseEntity.ok(assetToUpdate.toDto());
    }

    @DeleteMapping("/assets/{id}")
    public ResponseEntity<?> deleteAsset(@PathVariable("id") Long id){
        _log.info("Deleting asset with id: " + id);

        var asset = assetRepository.findById(id);

        if (asset.isEmpty()) {
            _log.info("No asset found with id: " + id);
            return ResponseEntity.notFound().build();
        }

        assetRepository.delete(asset.get());
        return ResponseEntity.noContent().build();
    }


}
