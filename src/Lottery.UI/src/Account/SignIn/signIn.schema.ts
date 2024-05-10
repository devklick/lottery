import { z } from "zod";
import { userTypeSchema } from "../../common/schemas";

export const signInRequestSchema = z.object({
  username: z.string().min(1, "Must contain a value"),
  password: z.string().min(1, "Must contain a value"),
  staySignedIn: z.boolean().optional().default(true),
});

export const signInResponseSchema = z.object({
  userType: userTypeSchema,
  sessionExpiry: z.string().pipe(z.coerce.date()),
});

export type SignInRequest = z.infer<typeof signInRequestSchema>;
export type SignInResponse = z.infer<typeof signInResponseSchema>;
