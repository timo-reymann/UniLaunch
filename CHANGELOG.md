## [1.5.2](https://github.com/timo-reymann/UniLaunch/compare/1.5.1...1.5.2) (2025-09-04)


### Bug Fixes

* **deps:** update avalonia monorepo to 11.3.5 ([#84](https://github.com/timo-reymann/UniLaunch/issues/84)) ([5ca5c6a](https://github.com/timo-reymann/UniLaunch/commit/5ca5c6a3da0f6633074aa9127fc3c6760f7ddc70))

## [1.5.1](https://github.com/timo-reymann/UniLaunch/compare/1.5.0...1.5.1) (2025-08-06)


### Bug Fixes

* **deps:** update avalonia monorepo to 11.3.3 ([#78](https://github.com/timo-reymann/UniLaunch/issues/78)) ([9dbd8bf](https://github.com/timo-reymann/UniLaunch/commit/9dbd8bf9a8b309881a75e62b7f9767e42caa3807))

## [1.5.0](https://github.com/timo-reymann/UniLaunch/compare/1.4.0...1.5.0) (2024-11-23)


### Features

* Various updates & performance improvements ([4dc5115](https://github.com/timo-reymann/UniLaunch/commit/4dc5115f0003b4412a14b17d1a807f2afdf11046))

## [1.4.0](https://github.com/timo-reymann/UniLaunch/compare/1.3.0...1.4.0) (2024-05-11)


### Features

* Add update notification for editor ([#14](https://github.com/timo-reymann/UniLaunch/issues/14)) ([5337375](https://github.com/timo-reymann/UniLaunch/commit/5337375e0861090191e0f4e3ca4f61860da79676))

## [1.3.0](https://github.com/timo-reymann/UniLaunch/compare/1.2.0...1.3.0) (2024-05-11)


### Features

* Add localization support ([#13](https://github.com/timo-reymann/UniLaunch/issues/13)) ([9855207](https://github.com/timo-reymann/UniLaunch/commit/985520799a72edb5ea95e94036d08cb824c8c17f))

## [1.2.0](https://github.com/timo-reymann/UniLaunch/compare/1.1.0...1.2.0) (2024-04-19)


### Features

* **#10:** Add self hosted connectivity check server ([7ab5d64](https://github.com/timo-reymann/UniLaunch/commit/7ab5d64d5bdb60b3f63b9b7f9f81f8f3cf1d34fc)), closes [#10](https://github.com/timo-reymann/UniLaunch/issues/10)

## [1.1.0](https://github.com/timo-reymann/UniLaunch/compare/1.0.0...1.1.0) (2024-04-18)


### Features

* **#8:** Add connectivity check functionality for targets ([4de54de](https://github.com/timo-reymann/UniLaunch/commit/4de54de7379597e76266a72a253bef103dfe59af)), closes [#8](https://github.com/timo-reymann/UniLaunch/issues/8) [#9](https://github.com/timo-reymann/UniLaunch/issues/9)
* Add connectivity toggle to target controls ([d8ecdce](https://github.com/timo-reymann/UniLaunch/commit/d8ecdce30b09cb65ad647ba1726c032e5c837100))
* Add network connectivity toggle control ([98b5125](https://github.com/timo-reymann/UniLaunch/commit/98b5125ce454d55b3bfad98f038261cd55a7b529))
* Add NetworkConnectivityChecker ([1f70102](https://github.com/timo-reymann/UniLaunch/commit/1f701025777bb985db9e4c20d7bbde987334ad11))
* Add URI converter to write urls properly ([64106e6](https://github.com/timo-reymann/UniLaunch/commit/64106e6b8fc48318d1b300f5e950784b5b7c7ba8))
* Extend settings dialog with connectivity config ([094fd13](https://github.com/timo-reymann/UniLaunch/commit/094fd13566b6057365b521c01ec7b22ff470df2b))
* Extend target to have possibility to depend on network connectivity ([838b99e](https://github.com/timo-reymann/UniLaunch/commit/838b99e2ac254d6d6c9d5031893ead0be59244b2))
* Split network depending on connectivity check and regular target startup ([433d7f7](https://github.com/timo-reymann/UniLaunch/commit/433d7f7067a9272eae008fb3dc9230c40d92e165))
* Sync settings changes back to main window unsaved ([617f05b](https://github.com/timo-reymann/UniLaunch/commit/617f05b8846205de4699f5e8897c638eeb165a3b))


### Bug Fixes

* Add base properties to view model dependency generation ([882f0d7](https://github.com/timo-reymann/UniLaunch/commit/882f0d7529ea504c14ab0985f7b47449ddda054c))
* Fix serialization error when endpoint and timeout are not set ([2d5ef92](https://github.com/timo-reymann/UniLaunch/commit/2d5ef9208968be1229a84baaebe3f7af69b8e35b))
* Fix target invoke result output ([7ff0af8](https://github.com/timo-reymann/UniLaunch/commit/7ff0af8aa6997d2c36cf2a7aba535ca1230e2c41))
* **viewmodel-generator:** Generate properties also for base model properties ([5a46972](https://github.com/timo-reymann/UniLaunch/commit/5a46972517998737378ff02c7baf81d37c15cc9f))

## [0.1.0](https://github.com/timo-reymann/UniLaunch/compare/0.0.7...0.1.0) (2024-03-13)


### Features

* Add ExclusiveInstanceProvider ([c950d19](https://github.com/timo-reymann/UniLaunch/commit/c950d198d831d37921e2ca87c02a8072e6e4171c))
* Add exclusivity acquire to all native entrypoints ([a88100c](https://github.com/timo-reymann/UniLaunch/commit/a88100cef5efd64b162a7ce952724509ed47d6e6))

## [0.0.7](https://github.com/timo-reymann/UniLaunch/compare/0.0.6...0.0.7) (2024-03-11)


### Bug Fixes

* Add icon to all non main windows ([57b8706](https://github.com/timo-reymann/UniLaunch/commit/57b8706907b4dfbe8f99c783819ef1f1a138dcb3))
* Fix binding for rule set rule count ([ad79e8d](https://github.com/timo-reymann/UniLaunch/commit/ad79e8de47749309002a5054ac32f4bf0924e573))
