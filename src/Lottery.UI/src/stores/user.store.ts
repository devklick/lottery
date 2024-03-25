import { create } from "zustand";
import { persist } from "zustand/middleware";

type UserType = "Guest" | "Basic" | "Admin";

interface UserStore {
  _authenticated: boolean;
  authenticated(): boolean;
  sessionExpiry: Date;
  userType: UserType;

  login(userType: UserType, sessionExpiry: Date): void;
  logout(): void;
  isUserType(userType: UserType): boolean;
}

export const useUserStore = create<UserStore>()(
  persist(
    (set, get) => ({
      _authenticated: false,
      userType: "Guest",
      sessionExpiry: new Date(0),
      authenticated() {
        if (!get()._authenticated) return false;
        if (get().sessionExpiry.getTime() < Date.now()) {
          set({ _authenticated: false, sessionExpiry: new Date(0) });
          return false;
        }
        return true;
      },
      login(userType, sessionExpiry) {
        set({ _authenticated: true, userType, sessionExpiry });
      },
      logout() {
        set({
          _authenticated: false,
          userType: "Guest",
          sessionExpiry: new Date(0),
        });
      },
      isUserType(userType) {
        return get().userType === userType;
      },
    }),
    {
      name: "user",
      merge(persisted, current) {
        const persistedUserStore = persisted as UserStore;
        if (persistedUserStore?.sessionExpiry) {
          current.sessionExpiry = new Date(persistedUserStore.sessionExpiry);
        }
        return current;
      },
    }
  )
);
