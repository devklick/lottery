import { z } from "zod";

import { userTypeSchema } from "../../../common/schemas";

export const signInRequestSchema = z.object({
  usernameOrEmail: z
    .string()
    .min(1, "A username or email address is required")
    .describe(
      "The username or email address that you registered your account with",
    ),
  password: z
    .string()
    .min(1, "A password is required")
    .describe("The password you used when creating your account"),
  staySignedIn: z
    .boolean()
    .optional()
    .default(true)
    .describe("Whether or not you login should be saved to the browser"),
});

export const signInResponseSchema = z.object({
  userType: userTypeSchema,
  sessionExpiry: z.string().pipe(z.coerce.date()),
});

export type SignInRequest = z.infer<typeof signInRequestSchema>;
export type SignInResponse = z.infer<typeof signInResponseSchema>;
