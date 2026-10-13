// What the Stage runtime renders this application from - the counterpart of a live Stage's
// /stage/scene, /stage/routes and /stage/strings endpoints. Generated; do not edit.
import type { StageSceneApplication } from '../.frontend/stage/App';
import type { StageStaticContent } from '../.frontend/stage/stageSource';
import scene from './stage-scene.json';
import './bindings';
import { IssueApiKey as __stageCommand0 } from '../Shop/Customers/IssueApiKey';
import { IssueInvite as __stageCommand1 } from '../Shop/Customers/IssueInvite';
import { PlaceOrder as __stageCommand2 } from '../Shop/Orders/PlaceOrder';
import { RegisterCustomer as __stageCommand3 } from '../Shop/Customers/RegisterCustomer';

export const stage: StageStaticContent = {
    scene: scene as unknown as StageSceneApplication,
    routes: {
        commands: {
            "IssueApiKey": new __stageCommand0().route,
            "IssueInvite": new __stageCommand1().route,
            "PlaceOrder": new __stageCommand2().route,
            "RegisterCustomer": new __stageCommand3().route,
        },
        queries: {
        },
    },
    strings: {},
};
