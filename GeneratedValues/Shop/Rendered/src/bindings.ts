// Registers the generated Arc proxies under the names the composed Scene refers to.
// Scene resolves bindings by name; routes stay owned by Arc at runtime.
import { registerCommands } from '@cratis/scene.components';
import { IssueApiKey } from '../Shop/Customers/IssueApiKey';
import { IssueInvite } from '../Shop/Customers/IssueInvite';
import { PlaceOrder } from '../Shop/Orders/PlaceOrder';
import { RegisterCustomer } from '../Shop/Customers/RegisterCustomer';

registerCommands({IssueApiKey, IssueInvite, PlaceOrder, RegisterCustomer});
