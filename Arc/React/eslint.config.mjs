// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import js from '@eslint/js';
import typescriptEslint from '@typescript-eslint/eslint-plugin';
import typescriptParser from '@typescript-eslint/parser';
import header from 'eslint-plugin-header';
import react from 'eslint-plugin-react';
import globals from 'globals';

export default [
    {
        ignores: ['bin/**', 'obj/**', 'wwwroot/**', 'node_modules/**'],
    },
    js.configs.recommended,
    {
        files: ['**/*.ts', '**/*.tsx'],
        languageOptions: {
            parser: typescriptParser,
            parserOptions: {
                ecmaFeatures: { jsx: true },
                ecmaVersion: 'latest',
                sourceType: 'module',
            },
            globals: {
                ...globals.browser,
                ...globals.node,
            },
        },
        plugins: {
            '@typescript-eslint': typescriptEslint,
            header,
            react,
        },
        rules: {
            ...typescriptEslint.configs.recommended.rules,
            '@typescript-eslint/no-explicit-any': 'error',
            '@typescript-eslint/no-unused-vars': ['error', { ignoreRestSiblings: true }],
            'header/header': 'off',
            'no-undef': 'off',
            'react/display-name': 'off',
            'react/react-in-jsx-scope': 'off',
        },
        settings: {
            react: { version: 'detect' },
        },
    },
];
