import { z } from "zod";

import { userTypeSchema } from "../../../common/schemas";

export const signInRequestSchema = z.object({
  usernameOrEmail: z.string().min(1, "A username or email address is required"),
  password: z.string().min(1, "A password is required"),
  staySignedIn: z.boolean().optional().default(true),
});

export const signInResponseSchema = z.object({
  userType: userTypeSchema,
  sessionExpiry: z.string().pipe(z.coerce.date()),
});

export type SignInRequest = z.infer<typeof signInRequestSchema>;
export type SignInResponse = z.infer<typeof signInResponseSchema>;
