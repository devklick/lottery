import { z } from "zod";

import { userTypeSchema } from "../../../common/schemas";

export const userInviteRequestBodySchema = z.object({
  email: z.string().email(),
  userType: userTypeSchema,
});

export const userInviteRequestSchema = z.object({
  body: userInviteRequestBodySchema,
});

export const userInviteResponseSchema = z.object({
  email: z.string(),
  token: z.string(),
});

export type UserInviteRequestBody = z.infer<typeof userInviteRequestBodySchema>;
export type UserInviteRequest = z.infer<typeof userInviteRequestSchema>;
export type UserInviteResponse = z.infer<typeof userInviteResponseSchema>;
