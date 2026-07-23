/* eslint-disable */
//prettier-ignore
module.exports = {
  name: `plugin-disable-typescript-compat`,
  factory: function (require) {
    const { structUtils } = require(`@yarnpkg/core`);
    return {
      hooks: {
        // Yarn's builtin `compat/typescript` patch rewrites the `typescript`
        // dependency to patch lib/_tsc.js, lib/tsserver.js, lib/typescript.js.
        // The TypeScript 7 native compiler ships only lib/tsc.js + bin/tsc, so
        // that builtin patch fails with ENOENT. This hook runs after the compat
        // plugin and unwraps the builtin patch back to the plain npm descriptor.
        reduceDependency: function (dependency) {
          if (
            dependency.name === `typescript` &&
            dependency.range.startsWith(`patch:`) &&
            dependency.range.includes(`builtin<compat/typescript>`)
          ) {
            const source = decodeURIComponent(
              structUtils.parseRange(dependency.range).source
            );
            const original = structUtils.parseDescriptor(source, true);
            return structUtils.makeDescriptor(dependency, original.range);
          }
          return dependency;
        },
      },
    };
  },
};
